using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebNeuralNets.Models.db
{
    public class WebNeuralNetDbContext : IdentityDbContext<ApplicationUser>
    {
        public WebNeuralNetDbContext(DbContextOptions options) : base(options)
        {
        }

        protected WebNeuralNetDbContext()
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
    }
}
