using WealthSync.Models;

namespace WealthSync.Data
{
    public class Saving
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Icon { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Contribution> Contributions { get; set; }

        public AppUser AppUser { get; set; }
    }
}
