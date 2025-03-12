using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WealthSync.Data;
using WealthSync.repository.interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SavingsGoalController : ControllerBase
{
    private readonly ISavingsRepository _savingsGoalRepository;

    public SavingsGoalController(ISavingsRepository savingsGoalRepository)
    {
        _savingsGoalRepository = savingsGoalRepository;
    }

    // GET: api/SavingsGoal
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SavingsGoalDto>>> GetSavingsGoals(int? limit)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var goals = await _savingsGoalRepository.GetByUserIdAsync(userId, limit);
        var goalDtos = goals.Select(g => new SavingsGoalDto
        {
            Id = g.Id,
            Name = g.Name,
            TargetAmount = g.TargetAmount,
            CurrentAmount = g.CurrentAmount,
            StartDate = g.StartDate,
            TargetDate = g.TargetDate
        });

        return Ok(goalDtos);
    }

    // GET: api/SavingsGoal/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SavingsGoalDetailDto>> GetSavingsGoal(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var goal = await _savingsGoalRepository.GetByIdForUserAsync(id, userId);
        if (goal == null) return NotFound();

        var goalDto = new SavingsGoalDetailDto
        {
            Id = goal.Id,
            Name = goal.Name,
            TargetAmount = goal.TargetAmount,
            CurrentAmount = goal.CurrentAmount,
            StartDate = goal.StartDate,
            TargetDate = goal.TargetDate,
            Contributions = goal.Contributions.Select(c => new ContributionDto
            {
                Id = c.Id,
                Amount = c.Amount,
                Date = c.Date,
                Description = c.Description
            }).ToList()
        };

        return Ok(goalDto);
    }

    // POST: api/SavingsGoal
    [HttpPost]
    public async Task<ActionResult<SavingsGoalDto>> CreateSavingsGoal(CreateSavingsDto createDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var goal = new Saving
        {
            AppUserId = userId,
            Name = createDto.Name,
            TargetAmount = createDto.TargetAmount,
            CurrentAmount = 0, // Starts at 0
            StartDate = createDto.StartDate,
            TargetDate = createDto.TargetDate
        };

        await _savingsGoalRepository.AddAsync(goal);

        var goalDto = new SavingsGoalDto
        {
            Id = goal.Id,
            Name = goal.Name,
            TargetAmount = goal.TargetAmount,
            CurrentAmount = goal.CurrentAmount,
            StartDate = goal.StartDate,
            TargetDate = goal.TargetDate
        };

        return CreatedAtAction(nameof(GetSavingsGoal), new { id = goal.Id }, goalDto);
    }

    // PUT: api/SavingsGoal/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSavingsGoal(int id, UpdateSavingsDto updateDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var goal = await _savingsGoalRepository.GetByIdForUserAsync(id, userId);
        if (goal == null) return NotFound();

        goal.Name = updateDto.Name;
        goal.TargetAmount = updateDto.TargetAmount;
        goal.StartDate = updateDto.StartDate;
        goal.TargetDate = updateDto.TargetDate;

        try
        {
            await _savingsGoalRepository.UpdateAsync(goal);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _savingsGoalRepository.ExistsAsync(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/SavingsGoal/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSavingsGoal(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var goal = await _savingsGoalRepository.GetByIdForUserAsync(id, userId);
        if (goal == null) return NotFound();

        await _savingsGoalRepository.DeleteAsync(goal);

        return NoContent();
    }
}
