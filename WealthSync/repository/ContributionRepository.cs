using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WealthSync.Data;
using WealthSync.repository.interfaces;

public class ContributionRepository : IContributionsRepository
{
    private readonly AppDbContext _context;

    public ContributionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contribution>> GetAllAsync()
    {
        return await _context.Contributions
            .Include(c => c.Saving)
            .ToListAsync();
    }

    public async Task<Contribution> GetByIdAsync(int id)
    {
        return await _context.Contributions
            .Include(c => c.Saving)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Contribution> GetByIdForUserAsync(int id, string userId)
    {
        return await _context.Contributions
            .Include(c => c.Saving)
            .FirstOrDefaultAsync(c => c.Id == id && c.Saving.AppUserId == userId);
    }

    public async Task<IEnumerable<Contribution>> GetBySavingsGoalIdAsync(int savingsGoalId, string userId)
    {
        return await _context.Contributions
            .Include(c => c.Saving)
            .Where(c => c.SavingId == savingsGoalId && c.Saving.AppUserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Contribution entity)
    {
        _context.Contributions.Add(entity);
        // Update Saving.CurrentAmount
        var goal = await _context.Savings.FindAsync(entity.SavingId);
        if (goal != null)
        {
            goal.CurrentAmount += entity.Amount;
            _context.Entry(goal).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contribution entity)
    {
        // Adjust Saving.CurrentAmount based on the difference
        var original = await _context.Contributions.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == entity.Id);
        var goal = await _context.Savings.FindAsync(entity.SavingId);
        if (original != null && goal != null)
        {
            goal.CurrentAmount = goal.CurrentAmount - original.Amount + entity.Amount;
            _context.Entry(goal).State = EntityState.Modified;
        }
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Contribution entity)
    {
        // Subtract from Saving.CurrentAmount
        var goal = await _context.Savings.FindAsync(entity.SavingId);
        if (goal != null)
        {
            goal.CurrentAmount -= entity.Amount;
            _context.Entry(goal).State = EntityState.Modified;
        }
        _context.Contributions.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Contributions.AnyAsync(c => c.Id == id);
    }
}