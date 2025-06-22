namespace WealthSync.Dtos;

public class BudgetCategoryDto
{
     public int Id { get; set; }
     public int BudgetId { get; set; }
     public string Name { get; set; }
     public double AllocatedAmount { get; set; }
     public double SpentAmount { get; set; }
}