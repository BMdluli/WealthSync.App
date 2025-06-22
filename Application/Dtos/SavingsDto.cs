using WealthSync.Data;
using WealthSync.Models;

namespace WealthSync.Dtos
{
    public class SavingsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Icon { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<ContributionDto> Contributions { get; set; }

        public string AppUserId { get; set; }
    }
}
