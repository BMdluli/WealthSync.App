using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WealthSync.Data;
using WealthSync.Dtos;
using WealthSync.Models;

namespace WealthSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthController(AppDbContext context, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
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

            return Ok("User created successfully");
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

            return Ok(token);

        }


        private string GenerateToken(AppUser user)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

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
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
