using Microsoft.EntityFrameworkCore;
using WealthSync.Data;
using WealthSync.Models;
using WealthSync.repository.interfaces;

namespace WealthSync.repository;

public class ContributionRepository: IContributionsRepository
{
    private readonly AppDbContext _context;

    public ContributionRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> AddContributionAsync(CreateContributionDto contributionDto, string userId)
    {
        var saving = await _context.Savings
            .Include(a => a.AppUser)
            .FirstOrDefaultAsync(x => x.Id == contributionDto.SavingId);

        if (saving == null)
        {
            return false;
        }
        
        // CHECK IF USER INDEED OWNS saving
        if (userId != saving.AppUser.Id)
        {
            return false;
        }

        var contributionToSave = new Contribution
        {
            Amount = contributionDto.Amount,
            icon = contributionDto.icon,
            SavingId = saving.Id
        };
        
        await _context.Contributions.AddAsync(contributionToSave);

        if (await _context.SaveChangesAsync() > 0)
        {
            return true;
        }
        
        return false;
    }

    public async Task<bool> RemoveContributionAsync(CreateContributionDto contributionDto, string userId)
    {
        var saving = await _context.Savings
            .Include(a => a.AppUser)
            .FirstOrDefaultAsync(x => x.Id == contributionDto.SavingId);

        if (saving == null)
        {
            return false;
        }
        
        // CHECK IF USER INDEED OWNS saving
        if (userId != saving.AppUser.Id)
        {
            return false;
        }

        var contributionToSave = new Contribution
        {
            Amount = -contributionDto.Amount,
            icon = contributionDto.icon,
            SavingId = saving.Id
        };
        
        await _context.Contributions.AddAsync(contributionToSave);

        if (await _context.SaveChangesAsync() > 0)
        {
            return true;
        }
        
        return false;
    }
    
}