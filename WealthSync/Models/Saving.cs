using System.ComponentModel.DataAnnotations;
using WealthSync.Models;

namespace WealthSync.Data
{
    public class Saving
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public double Amount { get; set; }
        [Required]
        public string Icon { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Contribution> Contributions { get; set; }

        public AppUser AppUser { get; set; }
    }
}
