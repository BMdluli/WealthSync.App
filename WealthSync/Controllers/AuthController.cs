using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WealthSync.Data;
using WealthSync.Dtos;
using WealthSync.Models;
using WealthSync.repository.interfaces;

namespace WealthSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AuthController(AppDbContext context, UserManager<AppUser> userManager, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            

            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return BadRequest("A user with that email already exists");
            }

            var user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Username,
                Name = registerDto.Name,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var response = new
            {
                message = "User created successfully"
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password)) {
                return Unauthorized("Invalid Credentials");
            }

            // user logged in
            var token = GenerateToken(user);

            var response = new
            {
                token = token
            };
            
            return Ok(response);

        }
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Ok(); // Don’t reveal user existence for security

            // Generate reset token
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)); // Secure random token
            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddHours(1), 
                IsUsed = false
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            // Send email
            var resetUrl = $"{_configuration["FrontendUrl"]}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(model.Email)}";
            var emailBody = $"<p>Click <a href=\"{resetUrl}\">here</a> to reset your password. This link expires in 1 hour.</p>";

            await _emailService.SendEmailAsync(model.Email, "Reset Your WealthSync Password", emailBody);

            return Ok();
        }
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new { message = "Invalid email or token" });

            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.UserId == user.Id && 
                                          t.Token == model.Token && 
                                          !t.IsUsed && 
                                          t.ExpiryDate > DateTime.UtcNow);

            if (resetToken == null)
                return BadRequest(new { message = "Invalid or expired token" });

            // Update password using Identity
            var identityToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, identityToken, model.NewPassword);
            if (!result.Succeeded)
                return BadRequest(new { message = "Password reset failed", errors = result.Errors });

            // Mark token as used
            resetToken.IsUsed = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password reset successfully" });
        }


        private string GenerateToken(AppUser user)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:SecretKey")));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new();
            claims.Add(new(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new(JwtRegisteredClaimNames.UniqueName, user.UserName));
            claims.Add(new(JwtRegisteredClaimNames.GivenName, user.Name));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(5),
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
