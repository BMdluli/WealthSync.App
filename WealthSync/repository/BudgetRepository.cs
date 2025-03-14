using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WealthSync.Data;
using WealthSync.Models;
using WealthSync.repository.interfaces;

public class BudgetRepository : IBudgetRepository
{
    private readonly AppDbContext _context;

    public BudgetRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Budget>> GetAllAsync()
    {
        return await _context.Budgets.ToListAsync();
    }

    public async Task<Budget> GetByIdAsync(int id)
    {
        return await _context.Budgets.FindAsync(id);
    }

    public async Task<Budget> GetByIdForUserAsync(int id, string userId)
    {
        return await _context.Budgets
            .Where(b => b.Id == id && b.AppUserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<Budget> GetDetailedByIdForUserAsync(int id, string userId)
    {
        return await _context.Budgets
            .Include(b => b.Categories)
            .ThenInclude(c => c.Expenses)
            .Where(b => b.Id == id && b.AppUserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetCountAsync(string userId)
    {
        return await _context.Budgets.Where(x => x.AppUserId == userId).CountAsync();
    }
    
    public async Task<IEnumerable<Budget>> GetByUserIdAsync(string userId, int? limit)
    {
        var budgets =  _context.Budgets
            .Where(b => b.AppUserId == userId)
            .AsQueryable();

        if (limit.HasValue)
        {
            budgets = budgets.Take(limit.Value);
        }
        
        return await budgets.ToListAsync();
    }

    public async Task AddAsync(Budget entity)
    {
        _context.Budgets.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Budget entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Budget entity)
    {
        _context.Budgets.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Budgets.AnyAsync(b => b.Id == id);
    }
}