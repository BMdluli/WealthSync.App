using System.ComponentModel.DataAnnotations;

namespace WealthSync.Dtos;

public class CreateStockDto
{
    [Required]
    public string Symbol { get; set; }
    [Required]
    [Range(0.01, double.MaxValue)]
    public double Shares { get; set; }
    [Required]
    [Range(0.01, double.MaxValue)]
    public double PurchasePrice { get; set; }
}