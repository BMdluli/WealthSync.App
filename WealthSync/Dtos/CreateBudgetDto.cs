namespace WealthSync.Controllers;

public class CreateBudgetDto
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double TotalIncome { get; set; }
}