using WealthSync.Models;

namespace WealthSync.Application.interfaces;

public interface IBudgetRepository : IRepository<Budget>
{
    Task<IEnumerable<Budget>> GetByUserIdAsync(string userId, int? limit);
    Task<Budget> GetByIdForUserAsync(int id, string userId);
    Task<Budget> GetDetailedByIdForUserAsync(int id, string userId); 
    Task<int> GetCountAsync(string userId);
}