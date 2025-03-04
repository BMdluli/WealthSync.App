using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WealthSync.Data;
using WealthSync.Dtos;
using WealthSync.repository.interfaces;

namespace WealthSync.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContributionsController : ControllerBase
    {
        private readonly IContributionsRepository _repository;
        
        public ContributionsController(IContributionsRepository repository)
        {
            _repository = repository;            
        }

        [HttpPost("add-contribution")]
        public async Task<IActionResult> AddContribution(CreateContributionDto contributionDto)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("Please login again.");
            }
            
            var result = await _repository.AddContributionAsync(contributionDto, userId);
            
            if (!result) return BadRequest("Something went wrong while adding contribution.");
            
            return Ok("Contribution added successfully.");
        }

        [HttpPost("remove-contribution")]
        public async Task<IActionResult> RemoveContribution(CreateContributionDto contributionDto)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("Please login again.");
            }
            
            var result = await _repository.RemoveContributionAsync(contributionDto, userId);
            
            if (!result) return BadRequest("Something went wrong while adding contribution.");
            
            return Ok("Contribution removed successfully.");
        }
        
        private string? GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            return userId;
        }
        
    }
}
