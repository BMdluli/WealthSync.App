using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WealthSync.Dtos;
using WealthSync.repository.interfaces;

namespace WealthSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingsController : ControllerBase
    {
        private readonly ISavingsRepository _savingsRepository;

        public SavingsController(ISavingsRepository savingsRepository)
        {
            _savingsRepository = savingsRepository;
        }

        [HttpPost("create-saving")]
        public async Task<ActionResult<string>> CreateSaving(CreateSavingsDto savings)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (userId == null) return Unauthorized("Please login and try again");

            var result = await _savingsRepository.CreateSavingsAsync(savings, userId);

            if (!result)
            {
                return BadRequest("Something went wrong while creating saving item");
            }


            return Ok("Saving item created successfully");
        }
    }
}
