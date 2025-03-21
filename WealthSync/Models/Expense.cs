namespace WealthSync.Models;

public class Expense
{
    public int Id { get; set; }
    
    public string Description { get; set; }
    
    public double Amount { get; set; }
    
    public DateTime Date { get; set; }
    
    public int BudgetCategoryId { get; set; }
    public BudgetCategory BudgetCategory { get; set; }
}