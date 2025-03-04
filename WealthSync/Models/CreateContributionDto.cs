namespace WealthSync.Models;

public class CreateContributionDto
{
    public double Amount { get; set; }
    public string icon { get; set; }
    public int SavingId { get; set; }
}