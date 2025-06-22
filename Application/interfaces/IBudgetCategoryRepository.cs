using WealthSync.Models;

namespace WealthSync.Application.interfaces;

public interface IBudgetCategoryRepository : IRepository<BudgetCategory>
{
    Task<IEnumerable<BudgetCategory>> GetByUserIdAsync(string userId);
    Task<IEnumerable<BudgetCategory>> GetByBudgetIdAsync(int budgetId, string userId);
    Task<BudgetCategory> GetByIdForUserAsync(int id, string userId);
}