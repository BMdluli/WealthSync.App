using WealthSync.Data;
using WealthSync.Dtos;

namespace WealthSync.Application.interfaces
{
    public interface ISavingsRepository : IRepository<Saving>
    {

        Task<IEnumerable<Saving>> GetByUserIdAsync(string userId, int? limit);
        Task<Saving> GetByIdForUserAsync(int id, string userId);
    }
}
