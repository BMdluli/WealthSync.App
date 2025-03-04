using WealthSync.Data;
using WealthSync.Dtos;

namespace WealthSync.repository.interfaces
{
    public interface ISavingsRepository
    {
        Task<IReadOnlyCollection<SavingsDto>> GetSavingsAsync(string userId);
        Task<SavingsDto?> GetSavingsByIdAsync(int id, string userId);
        Task<bool> CreateSavingsAsync(CreateSavingsDto createSavings, string userId);
        Task<bool> DeleteSavingsAsync(int id, string userId);
    }
}
