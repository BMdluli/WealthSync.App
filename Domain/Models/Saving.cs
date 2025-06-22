using System.ComponentModel.DataAnnotations;
using WealthSync.Models;

namespace WealthSync.Data
{
    public class Saving
    {
        public int Id { get; set; }

        [Required]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        [Required]
        public string Name { get; set; }
        
        public double TargetAmount { get; set; }
        
        public double CurrentAmount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? TargetDate { get; set; }

        public ICollection<Contribution> Contributions { get; set; }
    }
}
