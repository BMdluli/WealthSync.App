using System.ComponentModel.DataAnnotations;

namespace WealthSync.Dtos;

public class CreateContributionDto
{
    [Range(1, double.MaxValue)]
    public double Amount { get; set; }
    public int SavingId { get; set; }
}