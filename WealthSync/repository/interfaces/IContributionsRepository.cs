using WealthSync.Data;
using WealthSync.Dtos;

namespace WealthSync.repository.interfaces;

public interface IContributionsRepository : IRepository<Contribution>
{
    Task<IEnumerable<Contribution>> GetBySavingsGoalIdAsync(int savingsGoalId, string userId);
    Task<Contribution> GetByIdForUserAsync(int id, string userId);
}