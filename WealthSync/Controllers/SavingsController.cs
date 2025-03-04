using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WealthSync.Dtos;
using WealthSync.repository.interfaces;

namespace WealthSync.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SavingsController : ControllerBase
    {
        private readonly ISavingsRepository _savingsRepository;

        public SavingsController(ISavingsRepository savingsRepository)
        {
            _savingsRepository = savingsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<SavingsDto>>> GetSavings()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized("Please login again");

            var savings = await _savingsRepository.GetSavingsAsync(userId);
            
            return Ok(savings);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SavingsDto>> GetSavins(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized("Please login again");

            var saving = await _savingsRepository.GetSavingsByIdAsync(id, userId);
            
            if (saving == null) return NotFound("Saving not found");
            
            return Ok(saving);
        }

        [HttpPost("create-saving")]
        public async Task<ActionResult<string>> CreateSaving(CreateSavingsDto savings)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized("Please login again");

            var result = await _savingsRepository.CreateSavingsAsync(savings, userId);

            if (!result)
            {
                return BadRequest("Something went wrong while creating saving item");
            }


            return Ok("Saving item created successfully");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSavings(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized("Please login again");
            
            var result = await _savingsRepository.DeleteSavingsAsync(id, userId);
            
            if (!result) return BadRequest("Something went wrong deleting saving item");
            
            return Ok("Item deleted successfully");
        }

        private string? GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            return userId;
        }
    }
}
