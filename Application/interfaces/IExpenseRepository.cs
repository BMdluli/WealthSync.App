using WealthSync.Models;

namespace WealthSync.Application.interfaces;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<IEnumerable<Expense>> GetByCategoryIdAsync(int budgetCategoryId);
    Task<IEnumerable<Expense>> GetByUserIdAsync(string userId);
    Task<Expense> GetByIdForUserAsync(int id, string userId);
}