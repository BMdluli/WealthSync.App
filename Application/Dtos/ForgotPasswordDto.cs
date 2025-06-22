using System.ComponentModel.DataAnnotations;

namespace WealthSync.Dtos;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}