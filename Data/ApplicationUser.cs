using Microsoft.AspNetCore.Identity;

namespace API_BackEnd.Data
{
    public class ApplicationUser : IdentityUser
    {
        public required string Name { get; set; }
        
    }
}
