public class SavingsGoalDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double TargetAmount { get; set; }
    public double CurrentAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? TargetDate { get; set; }
}