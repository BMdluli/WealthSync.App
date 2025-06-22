public class CreateSavingsDto
{
    public string Name { get; set; }
    public double TargetAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? TargetDate { get; set; }
}