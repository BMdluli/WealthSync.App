using WealthSync.Data;
using WealthSync.Models;

namespace WealthSync.Dtos
{
    public class SavingsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Contribution> Contributions { get; set; }

        public string AppUserId { get; set; }
    }
}
