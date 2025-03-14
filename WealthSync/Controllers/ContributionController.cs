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
    private readonly IUnitOfWork _unitOfWork;

    public ContributionController(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/SavingsContribution
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContributionDto>>> GetContribution()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var Contribution = await _unitOfWork.Contribution.GetAllAsync();
        var userContribution = Contribution.Where(c => c.Saving.AppUserId == userId);
        var contributionDtos = userContribution.Select(c => new ContributionDto
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

        var contribution = await _unitOfWork.Contribution.GetByIdForUserAsync(id, userId);
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
    public async Task<ActionResult<IEnumerable<ContributionDto>>> GetContributionBySavingsGoal(int savingsGoalId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var Contribution = await _unitOfWork.Contribution.GetBySavingsGoalIdAsync(savingsGoalId, userId);
        var contributionDtos = Contribution.Select(c => new ContributionDto
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
        var goal = await _unitOfWork.Saving.GetByIdForUserAsync(createDto.SavingId, userId);
        if (goal == null) return BadRequest("Invalid SavingId");

        var contribution = new Contribution
        {
            SavingId = createDto.SavingId,
            Amount = createDto.Amount,
            Date = DateTime.UtcNow,
            Description = createDto.Description
        };

        await _unitOfWork.Contribution.AddAsync(contribution);

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

        var contribution = await _unitOfWork.Contribution.GetByIdForUserAsync(id, userId);
        if (contribution == null) return NotFound();

        // Validate new SavingId if changed
        if (updateDto.SavingId != contribution.SavingId)
        {
            var goal = await _unitOfWork.Saving.GetByIdForUserAsync(updateDto.SavingId, userId);
            if (goal == null) return BadRequest("Invalid SavingId");
            contribution.SavingId = updateDto.SavingId;
        }

        contribution.Amount = updateDto.Amount;
        contribution.Date = updateDto.Date;
        contribution.Description = updateDto.Description;

        try
        {
            await _unitOfWork.Contribution.UpdateAsync(contribution);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _unitOfWork.Contribution.ExistsAsync(id)) return NotFound();
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

        var contribution = await _unitOfWork.Contribution.GetByIdForUserAsync(id, userId);
        if (contribution == null) return NotFound();

        await _unitOfWork.Contribution.DeleteAsync(contribution);

        return NoContent();
    }
}
