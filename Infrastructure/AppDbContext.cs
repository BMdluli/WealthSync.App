using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WealthSync.Models;

namespace WealthSync.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Saving> Savings { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Dividend> Dividends { get; set; }
        public DbSet<StockPrice> StockPrices { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    }
}
