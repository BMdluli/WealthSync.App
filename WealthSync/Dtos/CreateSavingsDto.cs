using System.ComponentModel.DataAnnotations;

namespace WealthSync.Dtos
{
    public class CreateSavingsDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public double Amount { get; set; }
        [Required]
        public string Icon { get; set; }
    }
}
