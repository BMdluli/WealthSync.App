using System.ComponentModel.DataAnnotations;

namespace WealthSync.Data
{
    public class Contribution
    {
        public int Id { get; set; }

        [Required]
        public int SavingId { get; set; }
        public Saving Saving { get; set; }
        
        public double Amount { get; set; }

        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
