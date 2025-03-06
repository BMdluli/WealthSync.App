using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WealthSync.Data;

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
        IOptions<AlphaVantageOptions> options)
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

    public async Task<double> GetCurrentPriceAsync(string symbol)
    {
        string cacheKey = $"StockPrice:{symbol}";
        if (_cache.TryGetValue(cacheKey, out double cachedPrice))
        {
            return cachedPrice;
        }

        var httpClient = _httpClientFactory.CreateClient();
        string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={_apiKey}";
        var response = await httpClient.GetStringAsync(url);
        var data = JsonConvert.DeserializeObject<AlphaVantageTimeSeriesResponse>(response);

        if (data?.TimeSeriesDaily != null && data.TimeSeriesDaily.Count > 0)
        {
            var latestDate = data.TimeSeriesDaily.Keys.First();
            string priceString = data.TimeSeriesDaily[latestDate].Close;

            // Normalize by replacing ',' with '.' to ensure consistent decimal formatting
            priceString = priceString.Replace(',', '.');

            // Try parsing the price with the invariant culture
            if (double.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out double price))
            {
                _cache.Set(cacheKey, price, _cacheDuration);
                return price;
            }
            else
            {
                // Handle the case where the price cannot be parsed correctly
                Console.WriteLine($"Error parsing price: {priceString}");
                return 0;
            }
        }
        return 0;
    }


    public async Task<double> GetDividendYieldAsync(string symbol)
    {
        string cacheKey = $"DividendYield:{symbol}";
        if (_cache.TryGetValue(cacheKey, out double cachedYield))
        {
            return cachedYield;
        }

        var httpClient = _httpClientFactory.CreateClient();
        string url = $"https://www.alphavantage.co/query?function=OVERVIEW&symbol={symbol}&apikey={_apiKey}";
        var response = await httpClient.GetStringAsync(url);
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        if (data != null && data.TryGetValue("DividendYield", out var yieldStr) && double.TryParse(yieldStr, out var yield))
        {
            var yieldPercentage = yield * 100;
            _cache.Set(cacheKey, yieldPercentage, _cacheDuration);
            return yieldPercentage;
        }
        return 0;
    }

    public async Task<string> GetStockNameAsync(string symbol)
    {
        string cacheKey = $"StockName:{symbol}";
        if (_cache.TryGetValue(cacheKey, out string cachedName))
        {
            return cachedName;
        }

        var httpClient = _httpClientFactory.CreateClient();
        string url = $"https://www.alphavantage.co/query?function=OVERVIEW&symbol={symbol}&apikey={_apiKey}";
        var response = await httpClient.GetStringAsync(url);
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

        var name = data != null && data.TryGetValue("Name", out var n) ? n : "Unknown";
        _cache.Set(cacheKey, name, _cacheDuration);
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
}

public class AlphaVantageTimeSeriesResponse
{
    [JsonProperty("Time Series (Daily)")]
    public Dictionary<string, DailyData> TimeSeriesDaily { get; set; }
}

public class DailyData
{
    [JsonProperty("4. close")]
    public string Close { get; set; }
}

public class AlphaVantageOptions
{
    public string ApiKey { get; set; }
}