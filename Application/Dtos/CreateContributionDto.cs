using System.ComponentModel.DataAnnotations;

namespace WealthSync.Dtos;

public class CreateContributionDto
{
    [Required]
    public int SavingId { get; set; }
    [Required]
    [Range(0, double.MaxValue)]
    public double Amount { get; set; }
    public string Description { get; set; }
}