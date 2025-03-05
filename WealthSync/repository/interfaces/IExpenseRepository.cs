using Microsoft.AspNetCore.Mvc;
using WealthSync.Controllers;
using WealthSync.Dtos;
using WealthSync.Models;

namespace WealthSync.repository.interfaces;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<IEnumerable<Expense>> GetByCategoryIdAsync(int budgetCategoryId);
    Task<IEnumerable<Expense>> GetByUserIdAsync(string userId);
    Task<Expense> GetByIdForUserAsync(int id, string userId);
}