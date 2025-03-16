using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WealthSync.Data;
using WealthSync.Models;
using WealthSync.repository.interfaces;

namespace WealthSync.repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly IOptions<FmpOptions> _options;
    public IBudgetCategoryRepository BudgetCategory { get; }
    public IBudgetRepository Budget { get; }
    public IContributionsRepository Contribution { get; }
    public IExpenseRepository Expense { get; }
    public ISavingsRepository Saving { get; }
    public IStockRepository Stock { get; }

    public UnitOfWork(
        AppDbContext context,
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        IOptions<FmpOptions> options
        )
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _options = options;
        BudgetCategory = new BudgetCategoryRepository(context);
        Budget = new BudgetRepository(context);
        Contribution = new ContributionRepository(context);
        Expense = new ExpenseRepository(context);
        Saving = new SavingsRepository(context);
        Stock = new StockRepository(context, httpClientFactory, cache, options);
    }
    
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}