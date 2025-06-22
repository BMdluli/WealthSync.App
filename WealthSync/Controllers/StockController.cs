using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WealthSync.Application.interfaces;
using WealthSync.Dtos;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StockController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public StockController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/Stock
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StockDto>>> GetStocks()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var stocks = await _unitOfWork.Stock.GetByUserIdAsync(userId);
        var stockDtos = new List<StockDto>();

        foreach (var stock in stocks)
        {
            var currentPrice = await _unitOfWork.Stock.GetCurrentPriceAsync(stock.Symbol);
            if (currentPrice > 0 && !stock.StockPrices.Any(sp => sp.Timestamp.Date == DateTime.UtcNow.Date))
            {
                await _unitOfWork.Stock.UpdateStockDataAsync(stock.Id);
            }
            else
            {
                currentPrice = stock.StockPrices.OrderByDescending(sp => sp.Timestamp).FirstOrDefault()?.Price ?? 0;
            }

            var dividendYield = await _unitOfWork.Stock.GetDividendYieldAsync(stock.Symbol); // Now uses FMP
            var dividendFrequency = await _unitOfWork.Stock.GetDividendFrequencyAsync(stock.Symbol);

            stockDtos.Add(new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                Name = stock.Name,
                Shares = stock.Shares,
                PurchasePrice = stock.PurchasePrice,
                PurchaseDate = stock.PurchaseDate,
                CurrentPrice = currentPrice,
                DividendYield = dividendYield, // Directly from FMP as percentage
                TotalDividends = stock.Dividends.Sum(d => d.Amount * stock.Shares),
                DividendFrequency = dividendFrequency
            });
        }

        return Ok(stockDtos);
    }


    [HttpGet("/api/[controller]/prices")]
    public async Task<ActionResult<IEnumerable<StockOnlyDto>>> GetStocksWithoutDividends()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var stocks = await _unitOfWork.Stock.GetByUserIdAsync(userId);
        var stockDtos = new List<StockOnlyDto>();

        foreach (var stock in stocks)
        {
            var currentPrice = await _unitOfWork.Stock.GetCurrentPriceAsync(stock.Symbol);
            if (currentPrice > 0 && !stock.StockPrices.Any(sp => sp.Timestamp.Date == DateTime.UtcNow.Date))
            {
                await _unitOfWork.Stock.UpdateStockDataAsync(stock.Id);
            }
            else
            {
                currentPrice = stock.StockPrices.OrderByDescending(sp => sp.Timestamp).FirstOrDefault()?.Price ?? 0;
            }
            
            stockDtos.Add(new StockOnlyDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                Shares = stock.Shares,
                PurchasePrice = stock.PurchasePrice,
                CurrentPrice = currentPrice,
            });
        }

        return Ok(stockDtos);
    }

    // GET: api/Stock/5
    [HttpGet("{id}")]
    public async Task<ActionResult<StockDetailDto>> GetStock(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var stock = await _unitOfWork.Stock.GetByIdForUserAsync(id, userId);
        if (stock == null) return NotFound();

        var currentPrice = await _unitOfWork.Stock.GetCurrentPriceAsync(stock.Symbol);
        if (currentPrice > 0 && !stock.StockPrices.Any(sp => sp.Timestamp.Date == DateTime.UtcNow.Date))
        {
            await _unitOfWork.Stock.UpdateStockDataAsync(stock.Id);
        }
        else
        {
            currentPrice = stock.StockPrices.OrderByDescending(sp => sp.Timestamp).FirstOrDefault()?.Price ?? 0;
        }

        var dividendYield = await _unitOfWork.Stock.GetDividendYieldAsync(stock.Symbol);
        var dividendFrequency = await _unitOfWork.Stock.GetDividendFrequencyAsync(stock.Symbol);

        var stockDto = new StockDetailDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            Name = stock.Name,
            Shares = stock.Shares,
            PurchasePrice = stock.PurchasePrice,
            PurchaseDate = stock.PurchaseDate,
            CurrentPrice = currentPrice,
            DividendYield = dividendYield,
            TotalDividends = stock.Dividends.Sum(d => d.Amount * stock.Shares),
            DividendFrequency = dividendFrequency, // Add this
            Prices = stock.StockPrices.Select(sp => new StockPriceDto
            {
                Price = sp.Price,
                Timestamp = sp.Timestamp
            }).ToList(),
            Dividends = stock.Dividends.Select(d => new DividendDto
            {
                Amount = d.Amount,
                PaymentDate = d.PaymentDate
            }).ToList()
        };

        return Ok(stockDto);
    }

    // POST: api/Stock
    [HttpPost]
    public async Task<ActionResult<StockDto>> CreateStock(CreateStockDto createDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var userStocks = await _unitOfWork.Stock.GetByUserIdAsync(userId);
        if (userStocks.Count() >= 3)
        {
            return BadRequest("You have reached the maximum limit of 3 stocks. Delete an existing stock to add a new one.");
        }

        var stock = new Stock
        {
            UserId = userId,
            Symbol = createDto.Symbol.ToUpper(),
            Shares = createDto.Shares,
            PurchasePrice = createDto.PurchasePrice,
            PurchaseDate = DateTime.UtcNow
        };

        await _unitOfWork.Stock.AddAsync(stock);

        var currentPrice = await _unitOfWork.Stock.GetCurrentPriceAsync(stock.Symbol);
        var dividendYield = await _unitOfWork.Stock.GetDividendYieldAsync(stock.Symbol);

        var stockDto = new StockDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            Name = stock.Name,
            Shares = stock.Shares,
            PurchasePrice = stock.PurchasePrice,
            PurchaseDate = stock.PurchaseDate,
            CurrentPrice = currentPrice,
            DividendYield = dividendYield,
            TotalDividends = 0
        };

        return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, stockDto);
    }

    // DELETE: api/Stock/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStock(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var stock = await _unitOfWork.Stock.GetByIdForUserAsync(id, userId);
        if (stock == null) return NotFound("Stock not found or does not belong to you.");

        await _unitOfWork.Stock.DeleteAsync(stock);

        return NoContent();
    }
}