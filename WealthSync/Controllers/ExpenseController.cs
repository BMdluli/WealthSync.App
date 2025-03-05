using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WealthSync.Controllers;
using WealthSync.Data;
using WealthSync.Dtos;
using WealthSync.Models;
using WealthSync.repository.interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly AppDbContext _context; // Still needed for category validation

    public ExpenseController(IExpenseRepository expenseRepository, AppDbContext context)
    {
        _expenseRepository = expenseRepository;
        _context = context;
    }

    // GET: api/Expense
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpenses()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var expenses = await _expenseRepository.GetByUserIdAsync(userId);
        var expenseDtos = expenses.Select(e => new ExpenseDto
        {
            Id = e.Id,
            BudgetCategoryId = e.BudgetCategoryId,
            Description = e.Description,
            Amount = e.Amount,
            Date = e.Date
        });

        return Ok(expenseDtos);
    }

    // GET: api/Expense/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetExpense(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var expense = await _expenseRepository.GetByIdForUserAsync(id, userId);
        if (expense == null) return NotFound();

        var expenseDto = new ExpenseDto
        {
            Id = expense.Id,
            BudgetCategoryId = expense.BudgetCategoryId,
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date
        };

        return Ok(expenseDto);
    }

    // GET: api/Expense/BudgetCategory/5
    [HttpGet("BudgetCategory/{budgetCategoryId}")]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByCategory(int budgetCategoryId)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var expenses = await _expenseRepository.GetByCategoryIdAsync(budgetCategoryId);
        var filteredExpenses = expenses.Where(e => e.BudgetCategory.Budget.AppUserId == userId);
        var expenseDtos = filteredExpenses.Select(e => new ExpenseDto
        {
            Id = e.Id,
            BudgetCategoryId = e.BudgetCategoryId,
            Description = e.Description,
            Amount = e.Amount,
            Date = e.Date
        });

        return Ok(expenseDtos);
    }

    // POST: api/Expense
    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> CreateExpense(CreateExpenseDto createExpenseDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        // Validate BudgetCategory exists and belongs to the user
        var category = await _context.BudgetCategories
            .FirstOrDefaultAsync(c => c.Id == createExpenseDto.BudgetCategoryId && c.Budget.AppUserId == userId);
        if (category == null) return BadRequest("Invalid BudgetCategoryId");

        var expense = new Expense
        {
            BudgetCategoryId = createExpenseDto.BudgetCategoryId,
            Description = createExpenseDto.Description,
            Amount = createExpenseDto.Amount,
            Date = createExpenseDto.Date
        };

        await _expenseRepository.AddAsync(expense);

        var expenseDto = new ExpenseDto
        {
            Id = expense.Id,
            BudgetCategoryId = expense.BudgetCategoryId,
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date
        };

        return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expenseDto);
    }

    // PUT: api/Expense/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, UpdateExpenseDto updateExpenseDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var expense = await _expenseRepository.GetByIdForUserAsync(id, userId);
        if (expense == null) return NotFound();

        // Validate new BudgetCategoryId if changed
        if (updateExpenseDto.BudgetCategoryId != expense.BudgetCategoryId)
        {
            var category = await _context.BudgetCategories
                .FirstOrDefaultAsync(c => c.Id == updateExpenseDto.BudgetCategoryId && c.Budget.AppUserId == userId);
            if (category == null) return BadRequest("Invalid BudgetCategoryId");
            expense.BudgetCategoryId = updateExpenseDto.BudgetCategoryId;
        }

        expense.Description = updateExpenseDto.Description;
        expense.Amount = updateExpenseDto.Amount;
        expense.Date = updateExpenseDto.Date;

        try
        {
            await _expenseRepository.UpdateAsync(expense);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _expenseRepository.ExistsAsync(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Expense/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var expense = await _expenseRepository.GetByIdForUserAsync(id, userId);
        if (expense == null) return NotFound();

        await _expenseRepository.DeleteAsync(expense);

        return NoContent();
    }
}
