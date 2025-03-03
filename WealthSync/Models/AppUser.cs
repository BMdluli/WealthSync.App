using Microsoft.AspNetCore.Identity;

namespace WealthSync.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
