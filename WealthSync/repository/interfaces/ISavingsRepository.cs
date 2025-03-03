using WealthSync.Data;
using WealthSync.Dtos;

namespace WealthSync.repository.interfaces
{
    public interface ISavingsRepository
    {
        Task<IReadOnlyCollection<Saving>> GetSavingsAsync();
        Task<SavingsDto?> GetSavingsByIdAsync(int id);
        Task<bool> CreateSavingsAsync(CreateSavingsDto createSavings, string userId);
    }
}
