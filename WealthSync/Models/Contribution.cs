namespace WealthSync.Data
{
    public class Contribution
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string icon { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int SavingId { get; set; }
        public Saving Savings { get; set; }
    }
}
