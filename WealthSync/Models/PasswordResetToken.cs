namespace WealthSync.Models;

public class PasswordResetToken
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsUsed { get; set; }

    public AppUser User { get; set; }
}