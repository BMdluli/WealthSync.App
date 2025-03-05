namespace WealthSync.Controllers;

public class UpdateExpenseDto
{
    public int BudgetCategoryId { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }
    public DateTime Date { get; set; }
}