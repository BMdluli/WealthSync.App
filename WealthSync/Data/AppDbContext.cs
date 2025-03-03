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
    }
}
