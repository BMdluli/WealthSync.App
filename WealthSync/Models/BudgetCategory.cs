using System.ComponentModel.DataAnnotations;

namespace WealthSync.Models;

public class BudgetCategory
{
    public int Id { get; set; }
    
    [Required]
    public int BudgetId { get; set; }
    public Budget Budget { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public double AllocatedAmount { get; set; }
    
    public double SpentAmount { get; set; } // Optional: Could calculate from Expenses

    public ICollection<Expense> Expenses { get; set; }
}