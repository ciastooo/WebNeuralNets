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
        public DbSet<TranslationValue> TranslationValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.PhoneNumber);
            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.PhoneNumberConfirmed);
        }
    }
}
