using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WealthSync.Dtos;
using WealthSync.Models;
using WealthSync.repository.interfaces;


namespace WealthSync.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BudgetCategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public BudgetCategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/BudgetCategory
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetCategoryDto>>> GetBudgetCategories()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var categories = await _unitOfWork.BudgetCategory.GetByUserIdAsync(userId);
        var categoryDtos = categories.Select(c => new BudgetCategoryDto
        {
            Id = c.Id,
            BudgetId = c.BudgetId,
            Name = c.Name,
            AllocatedAmount = c.AllocatedAmount,
            SpentAmount = c.Expenses.Any() ? c.Expenses.Sum(e => e.Amount) : 0
        });

        return Ok(categoryDtos);
    }

    // GET: api/BudgetCategory/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetCategoryDto>> GetBudgetCategory(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var category = await _unitOfWork.BudgetCategory.GetByIdForUserAsync(id, userId);
        if (category == null) return NotFound();

        var categoryDto = new BudgetCategoryDto
        {
            Id = category.Id,
            BudgetId = category.BudgetId,
            Name = category.Name,
            AllocatedAmount = category.AllocatedAmount,
            SpentAmount = category.Expenses.Any() ? category.Expenses.Sum(e => e.Amount) : 0
        };

        return Ok(categoryDto);
    }

    // GET: api/BudgetCategory/Budget/5
    [HttpGet("Budget/{budgetId}")]
    public async Task<ActionResult<IEnumerable<BudgetCategoryDto>>> GetCategoriesByBudget(int budgetId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var categories = await _unitOfWork.BudgetCategory.GetByBudgetIdAsync(budgetId, userId);
        var categoryDtos = categories.Select(c => new BudgetCategoryDto
        {
            Id = c.Id,
            BudgetId = c.BudgetId,
            Name = c.Name,
            AllocatedAmount = c.AllocatedAmount,
            SpentAmount = c.Expenses.Any() ? c.Expenses.Sum(e => e.Amount) : 0
        });

        return Ok(categoryDtos);
    }

    // POST: api/BudgetCategory
    [HttpPost]
    public async Task<ActionResult<BudgetCategoryDto>> CreateBudgetCategory(CreateBudgetCategoryDto createDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        // Validate Budget exists and belongs to the user
        var budget = await _unitOfWork.Budget.GetByIdForUserAsync(createDto.BudgetId, userId);
        if (budget == null) return BadRequest("Invalid BudgetId");

        var category = new BudgetCategory
        {
            BudgetId = createDto.BudgetId,
            Name = createDto.Name,
            AllocatedAmount = createDto.AllocatedAmount
        };

        await _unitOfWork.BudgetCategory.AddAsync(category);

        var categoryDto = new BudgetCategoryDto
        {
            Id = category.Id,
            BudgetId = category.BudgetId,
            Name = category.Name,
            AllocatedAmount = category.AllocatedAmount,
            SpentAmount = 0 // No expenses yet
        };

        return CreatedAtAction(nameof(GetBudgetCategory), new { id = category.Id }, categoryDto);
    }

    // PUT: api/BudgetCategory/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudgetCategory(int id, UpdateBudgetCategoryDto updateDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var category = await _unitOfWork.BudgetCategory.GetByIdForUserAsync(id, userId);
        if (category == null) return NotFound();

        // Validate new BudgetId if changed
        if (updateDto.BudgetId != category.BudgetId)
        {
            var budget = await _unitOfWork.Budget.GetByIdForUserAsync(updateDto.BudgetId, userId);
            if (budget == null) return BadRequest("Invalid BudgetId");
            category.BudgetId = updateDto.BudgetId;
        }

        category.Name = updateDto.Name;
        category.AllocatedAmount = updateDto.AllocatedAmount;

        try
        {
            await _unitOfWork.BudgetCategory.UpdateAsync(category);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _unitOfWork.BudgetCategory.ExistsAsync(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/BudgetCategory/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudgetCategory(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var category = await _unitOfWork.BudgetCategory.GetByIdForUserAsync(id, userId);
        if (category == null) return NotFound();

        if (category.Expenses.Any()) return BadRequest("Cannot delete category with existing expenses.");

        await _unitOfWork.BudgetCategory.DeleteAsync(category);

        return NoContent();
    }
}