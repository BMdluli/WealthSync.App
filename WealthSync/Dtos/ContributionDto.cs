namespace WealthSync.Dtos;

public class ContributionDto
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public string icon { get; set; }
    public DateTime CreatedAt { get; set; }
}