using Microsoft.AspNetCore.Identity;
using WealthSync.Data;

namespace WealthSync.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }

        public List<Saving> Savings { get; set; } = new List<Saving>();
    }
}
