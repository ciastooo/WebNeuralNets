using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebNeuralNets.Models.DB
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
        public DbSet<NeuralNet> NeuralNets { get; set; }
        public DbSet<Layer> Layers { get; set; }
        public DbSet<Dendrite> Dendrites { get; set; }
        public DbSet<Neuron> Neurons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.PhoneNumber);
            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.PhoneNumberConfirmed);
            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.Email);
            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.EmailConfirmed);
            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.NormalizedEmail);
            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.TwoFactorEnabled);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.NeuralNets)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired();


            modelBuilder.Entity<TranslationValue>()
               .HasKey(t => new
               {
                   t.LanguageCode,
                   t.Key
               });


            modelBuilder.Entity<NeuralNet>()
                .HasMany(e => e.Layers)
                .WithOne(e => e.NeuralNet)
                .HasForeignKey(e => e.NeuralNetId)
                .IsRequired();
            

            modelBuilder.Entity<Layer>()
                .HasMany(e => e.Neurons)
                .WithOne(e => e.Layer)
                .HasForeignKey(e => e.LayerId)
                .IsRequired();


            modelBuilder.Entity<Neuron>()
                .HasMany(e => e.PreviousDendrites)
                .WithOne(e => e.NextNeuron)
                .HasForeignKey(e => e.NextNeuronId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Neuron>()
                .HasMany(e => e.NextDendrites)
                .WithOne(e => e.PreviousNeuron)
                .HasForeignKey(e => e.PreviousNeuronId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
