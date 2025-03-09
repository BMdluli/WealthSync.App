
using WealthSync.Data;
using WealthSync.repository.interfaces;

public interface IStockRepository : IRepository<Stock>
{
    Task<IEnumerable<Stock>> GetByUserIdAsync(string userId);
    Task<Stock> GetByIdForUserAsync(int id, string userId);
    Task<double> GetCurrentPriceAsync(string symbol);
    Task<double> GetDividendYieldAsync(string symbol);
    Task<string> GetStockNameAsync(string symbol);
    Task UpdateStockDataAsync(int stockId);
    Task<string> GetDividendFrequencyAsync(string stockSymbol);
}