using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WealthSync.Application.interfaces;
using WealthSync.Data;
using WealthSync.Models;

public class ExpenseRepository : IExpenseRepository
{
    private readonly AppDbContext _context;

    public ExpenseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Expense>> GetAllAsync()
    {
        return await _context.Expenses.ToListAsync();
    }

    public async Task<Expense> GetByIdAsync(int id)
    {
        return await _context.Expenses.FindAsync(id);
    }

    public async Task<Expense> GetByIdForUserAsync(int id, string userId)
    {
        return await _context.Expenses
            .Where(e => e.Id == id && e.BudgetCategory.Budget.AppUserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Expense>> GetByCategoryIdAsync(int budgetCategoryId)
    {
        return await _context.Expenses
            .Where(e => e.BudgetCategoryId == budgetCategoryId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByUserIdAsync(string userId)
    {
        return await _context.Expenses
            .Where(e => e.BudgetCategory.Budget.AppUserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Expense entity)
    {
        _context.Expenses.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Expense entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expense entity)
    {
        _context.Expenses.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Expenses.AnyAsync(e => e.Id == id);
    }
}