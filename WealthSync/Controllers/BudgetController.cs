using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WealthSync.Controllers;
using WealthSync.Dtos;
using WealthSync.Models;
using WealthSync.repository.interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public BudgetController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Budget
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetDto>>> GetBudgets(int? limit)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var budgets = await _unitOfWork.Budget.GetByUserIdAsync(userId, limit);
        var budgetDtos = budgets.Select(b => new BudgetDto
        {
            Id = b.Id,
            Name = b.Name,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            TotalIncome = b.TotalIncome
        });

        return Ok(budgetDtos);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<int>> GetBudgetCount()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();
        
        var count = await _unitOfWork.Budget.GetCountAsync(userId);

        var result = new
        {
            Count = count
        };
        
        return Ok(result);
    }

    // GET: api/Budget/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetDetailDto>> GetBudget(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var budget = await _unitOfWork.Budget.GetDetailedByIdForUserAsync(id, userId);
        if (budget == null) return NotFound();

        var budgetDto = new BudgetDetailDto
        {
            Id = budget.Id,
            Name = budget.Name,
            StartDate = budget.StartDate,
            EndDate = budget.EndDate,
            TotalIncome = budget.TotalIncome,
            Categories = budget.Categories.Select(c => new BudgetCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                AllocatedAmount = c.AllocatedAmount,
                SpentAmount = c.Expenses.Sum(e => e.Amount)
            }).ToList()
        };

        return Ok(budgetDto);
    }

    // POST: api/Budget
    [HttpPost]
    public async Task<ActionResult<BudgetDto>> CreateBudget(CreateBudgetDto createBudgetDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var budget = new Budget
        {
            AppUserId = userId,
            Name = createBudgetDto.Name,
            StartDate = createBudgetDto.StartDate,
            EndDate = createBudgetDto.EndDate,
            TotalIncome = createBudgetDto.TotalIncome
        };

        await _unitOfWork.Budget.AddAsync(budget);

        var budgetDto = new BudgetDto
        {
            Id = budget.Id,
            Name = budget.Name,
            StartDate = budget.StartDate,
            EndDate = budget.EndDate,
            TotalIncome = budget.TotalIncome
        };

        return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budgetDto);
    }

    // PUT: api/Budget/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudget(int id, UpdateBudgetDto updateBudgetDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var budget = await _unitOfWork.Budget.GetByIdForUserAsync(id, userId);
        if (budget == null) return NotFound();

        budget.Name = updateBudgetDto.Name;
        budget.StartDate = updateBudgetDto.StartDate;
        budget.EndDate = updateBudgetDto.EndDate;
        budget.TotalIncome = updateBudgetDto.TotalIncome;

        try
        {
            await _unitOfWork.Budget.UpdateAsync(budget);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _unitOfWork.Budget.ExistsAsync(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Budget/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var budget = await _unitOfWork.Budget.GetByIdForUserAsync(id, userId);
        if (budget == null) return NotFound();

        await _unitOfWork.Budget.DeleteAsync(budget);

        return NoContent();
    }
}
