using System.ComponentModel.DataAnnotations;

public class UpdateContributionDto
{
    [Required]
    public int SavingId { get; set; }
    [Required]
    [Range(0, double.MaxValue)]
    public double Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
}