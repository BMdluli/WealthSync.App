using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Globalization;
using WealthSync.Data;
using WealthSync.Models;

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly string _apiKey;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(1); // Cache for 1 hour

    public StockRepository(
        AppDbContext context,
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        IOptions<FmpOptions> options)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _apiKey = options.Value.ApiKey;
    }

    public async Task<IEnumerable<Stock>> GetAllAsync()
    {
        return await _context.Stocks
            .Include(s => s.StockPrices)
            .Include(s => s.Dividends)
            .ToListAsync();
    }

    public async Task<Stock> GetByIdAsync(int id)
    {
        return await _context.Stocks
            .Include(s => s.StockPrices)
            .Include(s => s.Dividends)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stock> GetByIdForUserAsync(int id, string userId)
    {
        return await _context.Stocks
            .Include(s => s.StockPrices)
            .Include(s => s.Dividends)
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
    }

    public async Task<IEnumerable<Stock>> GetByUserIdAsync(string userId)
    {
        return await _context.Stocks
            .Include(s => s.StockPrices)
            .Include(s => s.Dividends)
            .Where(s => s.UserId == userId)
            .ToListAsync();
    }

    private async Task<(double price, double dividendYield, string name)> FetchStockDataAsync(string symbol)
    {
        string cacheKeyPrice = $"StockPrice:{symbol}";
        string cacheKeyYield = $"DividendYield:{symbol}";
        string cacheKeyName = $"StockName:{symbol}";

        if (_cache.TryGetValue(cacheKeyPrice, out double cachedPrice) &&
            _cache.TryGetValue(cacheKeyYield, out double cachedYield) &&
            _cache.TryGetValue(cacheKeyName, out string cachedName))
        {
            return (cachedPrice, cachedYield, cachedName);
        }

        var httpClient = _httpClientFactory.CreateClient();
        string quoteUrl = $"https://financialmodelingprep.com/api/v3/quote/{symbol}?apikey={_apiKey}";
        var quoteResponse = await httpClient.GetStringAsync(quoteUrl);
        var quoteData = JsonConvert.DeserializeObject<List<FmpQuoteResponse>>(quoteResponse);

        if (quoteData != null && quoteData.Count > 0)
        {
            var quote = quoteData[0];
            double price = quote.Price;
            double dividendYield = quote.DividendYield ?? await CalculateDividendYieldAsync(symbol, price);
            string name = quote.Name ?? "Unknown";

            _cache.Set(cacheKeyPrice, price, _cacheDuration);
            _cache.Set(cacheKeyYield, dividendYield, _cacheDuration);
            _cache.Set(cacheKeyName, name, _cacheDuration);

            return (price, dividendYield, name);
        }

        return (0, 0, "Unknown");
    }

    public async Task<double> GetCurrentPriceAsync(string symbol)
    {
        var (price, _, _) = await FetchStockDataAsync(symbol);
        return price;
    }

    public async Task<double> GetDividendYieldAsync(string symbol)
    {
        var (_, dividendYield, _) = await FetchStockDataAsync(symbol);
        return dividendYield;
    }

    public async Task<string> GetStockNameAsync(string symbol)
    {
        var (_, _, name) = await FetchStockDataAsync(symbol);
        return name;
    }

    public async Task UpdateStockDataAsync(int stockId)
    {
        var stock = await GetByIdAsync(stockId);
        if (stock == null) return;

        var currentPrice = await GetCurrentPriceAsync(stock.Symbol);
        if (currentPrice > 0 && !stock.StockPrices.Any(sp => sp.Timestamp.Date == DateTime.UtcNow.Date))
        {
            var stockPrice = new StockPrice
            {
                StockId = stockId,
                Price = currentPrice,
                Timestamp = DateTime.UtcNow
            };
            _context.StockPrices.Add(stockPrice);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddAsync(Stock entity)
    {
        entity.Name = await GetStockNameAsync(entity.Symbol);
        entity.DividendFrequency = "Quarterly"; // Default assumption
        _context.Stocks.Add(entity);
        await _context.SaveChangesAsync();
        await UpdateStockDataAsync(entity.Id);
    }

    public async Task UpdateAsync(Stock entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Stock entity)
    {
        _context.Stocks.Remove(entity);
        await _context.SaveChangesAsync();
        _cache.Remove($"StockPrice:{entity.Symbol}");
        _cache.Remove($"DividendYield:{entity.Symbol}");
        _cache.Remove($"StockName:{entity.Symbol}");
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Stocks.AnyAsync(s => s.Id == id);
    }

    public async Task<string> GetDividendFrequencyAsync(string stockSymbol)
    {
        if (stockSymbol.ToUpper() == "O") return "Monthly";

        var stock = await _context.Stocks
            .Include(s => s.Dividends)
            .FirstOrDefaultAsync(s => s.Symbol == stockSymbol);

        if (stock != null && stock.Dividends?.Count > 1)
        {
            var lastFullYear = DateTime.UtcNow.Year - 1; 
            var dates = stock.Dividends
                .Where(d => d.PaymentDate.Year == lastFullYear)
                .Select(d => d.PaymentDate)
                .OrderBy(d => d)
                .ToList();

            if (dates.Count > 1)
            {
                var intervals = dates.Zip(dates.Skip(1), (d1, d2) => (d2 - d1).Days).Average();
                if (intervals <= 45) return "Monthly";
                if (intervals <= 120) return "Quarterly";
                if (intervals <= 240) return "Semi-Annually";
                return "Annually";
            }
        }
        return "Quarterly"; // Default
    }

    private async Task<double> CalculateDividendYieldAsync(string symbol, double price)
    {
        string cacheKey = $"DividendHistory:{symbol}";
        if (_cache.TryGetValue(cacheKey, out double cachedAnnualDividend))
        {
            return (cachedAnnualDividend / price) * 100;
        }

        var httpClient = _httpClientFactory.CreateClient();
        string dividendUrl = $"https://financialmodelingprep.com/api/v3/historical-price-full/stock_dividend/{symbol}?apikey={_apiKey}";
        var dividendResponse = await httpClient.GetStringAsync(dividendUrl);
        var dividendData = JsonConvert.DeserializeObject<FmpDividendResponse>(dividendResponse);

        if (dividendData?.Historical != null && dividendData.Historical.Count >= 4)
        {
            // Sum last 4 dividends (assuming quarterly)
            double annualDividend = dividendData.Historical
                .Take(4)
                .Sum(d => d.AdjDividend);
            _cache.Set(cacheKey, annualDividend, _cacheDuration);
            return (annualDividend / price) * 100;
        }

        return 0; // Fallback if insufficient data
    }
}

// FMP response model
public class FmpQuoteResponse
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("price")]
    public double Price { get; set; }

    [JsonProperty("dividendYield")]
    public double? DividendYield { get; set; } // Percentage, nullable

    [JsonProperty("name")]
    public string Name { get; set; }
}


public class FmpDividendResponse
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("historical")]
    public List<FmpDividendEntry> Historical { get; set; }
}

public class FmpDividendEntry
{
    [JsonProperty("date")]
    public string Date { get; set; }

    [JsonProperty("dividend")]
    public double Dividend { get; set; }

    [JsonProperty("adjDividend")] // Adjusted for splits
    public double AdjDividend { get; set; }
}

//public class AlphaVantageTimeSeriesResponse
//{
//    [JsonProperty("Time Series (Daily)")]
//    public Dictionary<string, DailyData> TimeSeriesDaily { get; set; }
//}

//public class DailyData
//{
//    [JsonProperty("4. close")]
//    public string Close { get; set; }
//}

//public class AlphaVantageOptions
//{
//    public string ApiKey { get; set; }
//}