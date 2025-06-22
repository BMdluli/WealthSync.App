using WealthSync.Dtos;

namespace WealthSync.Controllers;

public class BudgetDetailDto : BudgetDto
{
    public List<BudgetCategoryDto> Categories { get; set; }
}