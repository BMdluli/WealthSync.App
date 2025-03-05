using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WealthSync.Data;
using WealthSync.Models;
using WealthSync.repository.interfaces;

public class BudgetCategoryRepository : IBudgetCategoryRepository
{
    private readonly AppDbContext _context;

    public BudgetCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BudgetCategory>> GetAllAsync()
    {
        return await _context.BudgetCategories
            .Include(c => c.Expenses)
            .ToListAsync();
    }

    public async Task<BudgetCategory> GetByIdAsync(int id)
    {
        return await _context.BudgetCategories
            .Include(c => c.Expenses)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<BudgetCategory> GetByIdForUserAsync(int id, string userId)
    {
        return await _context.BudgetCategories
            .Include(c => c.Expenses)
            .FirstOrDefaultAsync(c => c.Id == id && c.Budget.AppUserId == userId);
    }

    public async Task<IEnumerable<BudgetCategory>> GetByUserIdAsync(string userId)
    {
        return await _context.BudgetCategories
            .Include(c => c.Expenses)
            .Where(c => c.Budget.AppUserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<BudgetCategory>> GetByBudgetIdAsync(int budgetId, string userId)
    {
        return await _context.BudgetCategories
            .Include(c => c.Expenses)
            .Where(c => c.BudgetId == budgetId && c.Budget.AppUserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(BudgetCategory entity)
    {
        _context.BudgetCategories.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BudgetCategory entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(BudgetCategory entity)
    {
        _context.BudgetCategories.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.BudgetCategories.AnyAsync(c => c.Id == id);
    }
}