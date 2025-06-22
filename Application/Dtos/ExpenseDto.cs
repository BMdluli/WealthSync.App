namespace WealthSync.Dtos;

public class ExpenseDto
{
    public int Id { get; set; }
    public int BudgetCategoryId { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }
    public DateTime Date { get; set; }
}