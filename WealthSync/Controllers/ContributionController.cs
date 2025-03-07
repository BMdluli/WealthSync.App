using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WealthSync.Data;
using WealthSync.Dtos;
using WealthSync.repository.interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ContributionController : ControllerBase
{
    private readonly IContributionsRepository _contributionRepository;
    private readonly ISavingsRepository _savingsGoalRepository;

    public ContributionController(
        IContributionsRepository contributionRepository,
        ISavingsRepository savingsGoalRepository)
    {
        _contributionRepository = contributionRepository;
        _savingsGoalRepository = savingsGoalRepository;
    }

    // GET: api/SavingsContribution
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContributionDto>>> GetContributions()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var contributions = await _contributionRepository.GetAllAsync();
        var userContributions = contributions.Where(c => c.Saving.AppUserId == userId);
        var contributionDtos = userContributions.Select(c => new ContributionDto
        {
            Id = c.Id,
            SavingId = c.SavingId,
            Amount = c.Amount,
            Date = c.Date,
            Description = c.Description
        });

        return Ok(contributionDtos);
    }

    // GET: api/SavingsContribution/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ContributionDto>> GetContribution(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var contribution = await _contributionRepository.GetByIdForUserAsync(id, userId);
        if (contribution == null) return NotFound();

        var contributionDto = new ContributionDto
        {
            Id = contribution.Id,
            SavingId = contribution.SavingId,
            Amount = contribution.Amount,
            Date = contribution.Date,
            Description = contribution.Description
        };

        return Ok(contributionDto);
    }

    // GET: api/SavingsContribution/SavingsGoal/5
    [HttpGet("SavingsGoal/{savingsGoalId}")]
    public async Task<ActionResult<IEnumerable<ContributionDto>>> GetContributionsBySavingsGoal(int savingsGoalId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var contributions = await _contributionRepository.GetBySavingsGoalIdAsync(savingsGoalId, userId);
        var contributionDtos = contributions.Select(c => new ContributionDto
        {
            Id = c.Id,
            SavingId = c.SavingId,
            Amount = c.Amount,
            Date = c.Date,
            Description = c.Description
        });

        return Ok(contributionDtos);
    }

    // POST: api/SavingsContribution
    [HttpPost]
    public async Task<ActionResult<ContributionDto>> CreateContribution(CreateContributionDto createDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        // Validate SavingsGoal exists and belongs to the user
        var goal = await _savingsGoalRepository.GetByIdForUserAsync(createDto.SavingId, userId);
        if (goal == null) return BadRequest("Invalid SavingId");

        var contribution = new Contribution
        {
            SavingId = createDto.SavingId,
            Amount = createDto.Amount,
            Date = DateTime.UtcNow,
            Description = createDto.Description
        };

        await _contributionRepository.AddAsync(contribution);

        var contributionDto = new ContributionDto
        {
            Id = contribution.Id,
            SavingId = contribution.SavingId,
            Amount = contribution.Amount,
            Date = contribution.Date,
            Description = contribution.Description
        };

        return CreatedAtAction(nameof(GetContribution), new { id = contribution.Id }, contributionDto);
    }

    // PUT: api/SavingsContribution/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContribution(int id, UpdateContributionDto updateDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var contribution = await _contributionRepository.GetByIdForUserAsync(id, userId);
        if (contribution == null) return NotFound();

        // Validate new SavingId if changed
        if (updateDto.SavingId != contribution.SavingId)
        {
            var goal = await _savingsGoalRepository.GetByIdForUserAsync(updateDto.SavingId, userId);
            if (goal == null) return BadRequest("Invalid SavingId");
            contribution.SavingId = updateDto.SavingId;
        }

        contribution.Amount = updateDto.Amount;
        contribution.Date = updateDto.Date;
        contribution.Description = updateDto.Description;

        try
        {
            await _contributionRepository.UpdateAsync(contribution);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _contributionRepository.ExistsAsync(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/SavingsContribution/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContribution(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var contribution = await _contributionRepository.GetByIdForUserAsync(id, userId);
        if (contribution == null) return NotFound();

        await _contributionRepository.DeleteAsync(contribution);

        return NoContent();
    }
}
