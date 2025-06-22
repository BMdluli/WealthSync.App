namespace WealthSync.Application.interfaces;

public interface IUnitOfWork: IDisposable
{
    IBudgetCategoryRepository BudgetCategory { get; }
    IBudgetRepository Budget { get; }
    IContributionsRepository Contribution { get; }
    IExpenseRepository Expense { get; }
    ISavingsRepository Saving { get; }
    IStockRepository Stock { get; }
    Task<int> CompleteAsync();
    
}