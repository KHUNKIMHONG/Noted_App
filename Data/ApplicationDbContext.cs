using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API_BackEnd.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // DbSets for your entities (e.g., Notes, Users)
        public DbSet<Note> Notes { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }  // Assuming you're using ASP.NET Identity for User management
    }
}
