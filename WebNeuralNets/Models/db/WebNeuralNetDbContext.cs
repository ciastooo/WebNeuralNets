﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
        public DbSet<TrainingData> TrainingData { get; set; }
        public DbQuery<QueryNeuralNet> NeuralNetQuery { get; set; }

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


            modelBuilder.Entity<TranslationValue>()
               .HasKey(t => new
               {
                   t.LanguageCode,
                   t.Key
               });


            modelBuilder.Entity<NeuralNet>()
               .HasOne(e => e.User)
               .WithMany(e => e.NeuralNets)
               .HasForeignKey(e => e.UserId)
               .IsRequired();


            modelBuilder.Entity<Layer>()
                .HasOne(e => e.NeuralNet)
                .WithMany(e => e.Layers)
                .HasForeignKey(e => e.NeuralNetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Neuron>()
                .HasOne(e => e.Layer)
                .WithMany(e => e.Neurons)
                .HasForeignKey(e => e.LayerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Neuron>()
                .HasMany(e => e.PreviousDendrites)
                .WithOne(e => e.NextNeuron)
                .HasForeignKey(e => e.NextNeuronId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Neuron>()
                .HasMany(e => e.NextDendrites)
                .WithOne(e => e.PreviousNeuron)
                .HasForeignKey(e => e.PreviousNeuronId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TrainingData>()
                .HasOne(e => e.NeuralNet)
                .WithMany(e => e.TrainingData)
                .HasForeignKey(e => e.NeuralNetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TrainingData>()
                .Property(e => e.TrainingSet)
                .HasConversion(
                    e => JsonConvert.SerializeObject(e),
                    e => JsonConvert.DeserializeObject<IList<TrainingSet>>(e));
        }

        public Task<List<NeuralNet>> FetchNeuralnetsToTrain()
        {
            //NeuralNets.FromSql()
            return NeuralNets.Where(nn => nn.Training && nn.TrainingData.Count > 0)
                             .Include(nn => nn.TrainingData)
                             .Include(nn => nn.Layers)
                                .ThenInclude(l => l.Neurons)
                                    .ThenInclude(n => n.PreviousDendrites)
                             .Include(nn => nn.Layers)
                                .ThenInclude(l => l.Neurons)
                                    .ThenInclude(n => n.NextDendrites)
                             .ToListAsync();
        } 

        public List<QueryNeuralNet> FetchNeuralNets(string userId)
        {
            var user = new SqlParameter("@userId", userId);
            return NeuralNetQuery.FromSql(@"
                  SELECT nn.[Id]
                      ,nn.[Name]
                      ,nn.[Description]
                      ,nn.[TrainingRate]
                      ,nn.[Training]
                      ,nn.[TrainingIterations]
	                  ,l.LayersCount
	                  ,(SELECT COUNT(*) FROM Neurons n Where n.LayerId IN (SELECT l.Id FROM Layers l WHERE l.NeuralNetId = nn.Id)) as NeuronsCount
                  FROM [WebNeuralNets].[dbo].[NeuralNets] nn
                  LEFT OUTER JOIN
	                  (SELECT l.NeuralNetId, Count(*) as LayersCount
	                  FROM Layers l
	                  Group By l.NeuralNetId)
                  l on nn.id = l.NeuralNetId
                  WHERE nn.[UserId] = @userId", user).ToList();
        }
    }
}
