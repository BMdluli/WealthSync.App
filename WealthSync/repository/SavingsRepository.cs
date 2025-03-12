using Microsoft.EntityFrameworkCore;

using WealthSync.Data;
using WealthSync.repository.interfaces;

public class SavingsRepository : ISavingsRepository
{
    private readonly AppDbContext _context;

    public SavingsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Saving>> GetAllAsync()
    {
        return await _context.Savings
            .Include(s => s.Contributions)
            .ToListAsync();
    }

    public async Task<Saving> GetByIdAsync(int id)
    {
        return await _context.Savings
            .Include(s => s.Contributions)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Saving> GetByIdForUserAsync(int id, string userId)
    {
        return await _context.Savings
            .Include(s => s.Contributions)
            .FirstOrDefaultAsync(s => s.Id == id && s.AppUserId == userId);
    }

    public async Task<IEnumerable<Saving>> GetByUserIdAsync(string userId, int? limit)
    {
        var savings =  _context.Savings
            .Include(s => s.Contributions)
            .Where(s => s.AppUserId == userId)
            .AsQueryable();

        if (limit.HasValue)
        {
            savings = savings.Take(limit.Value);
        }
        
        return await savings.ToListAsync();
        
    }

    public async Task AddAsync(Saving entity)
    {
        _context.Savings.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Saving entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Saving entity)
    {
        _context.Savings.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Savings.AnyAsync(s => s.Id == id);
    }
}