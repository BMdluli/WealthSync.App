using WealthSync.Data;
using WealthSync.Dtos;

namespace WealthSync.repository.interfaces
{
    public interface ISavingsRepository : IRepository<Saving>
    {

        Task<IEnumerable<Saving>> GetByUserIdAsync(string userId);
        Task<Saving> GetByIdForUserAsync(int id, string userId);
    }
}
