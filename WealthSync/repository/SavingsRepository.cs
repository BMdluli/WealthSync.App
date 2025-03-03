using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WealthSync.Data;
using WealthSync.Dtos;
using WealthSync.Models;
using WealthSync.repository.interfaces;

namespace WealthSync.repository
{
    public class SavingsRepository : ISavingsRepository
    {

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public SavingsRepository(AppDbContext context, UserManager<AppUser> userManager) 
        {
            _context = context; 
            _userManager = userManager;
        }

        public async Task<bool> CreateSavingsAsync(CreateSavingsDto createSavings, string userId)
        {
            Console.WriteLine(userId);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var saving = new Saving
            {
                Name = createSavings.Name,
                Amount = createSavings.Amount,
                AppUser = user,
            };

            await _context.AddAsync(saving);

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;

        }

        public async Task<IReadOnlyCollection<Saving>> GetSavingsAsync()
        {
            return await _context.Savings.ToListAsync();
        }

        public async  Task<SavingsDto?> GetSavingsByIdAsync(int id)
        {
           var saving = await _context.Savings.FirstOrDefaultAsync(x => x.Id == id);

            if (saving == null)
            {
                return null;
            }

            return new SavingsDto
            {
                Id = id,
                Name = saving.Name,
                Amount = saving.Amount,
                CreatedAt = saving.CreatedAt,

                Contributions = saving.Contributions,
            };

        }
    }
}
