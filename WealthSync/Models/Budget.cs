using Microsoft.Build.Framework;

namespace WealthSync.Models;

public class Budget
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public double TotalIncome { get; set; }

    public ICollection<BudgetCategory> Categories { get; set; }
    
    [Required]
    public string AppUserId { get; set; } // Assumes ASP.NET Identity uses string IDs
    public AppUser AppUser { get; set; }
}