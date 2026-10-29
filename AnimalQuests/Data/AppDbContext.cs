using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AnimalQuests.Models;

//Add-Migration name
//Update-Database

namespace AnimalQuests.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Biome> Biomes { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<AnimalMission> AnimalMissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AnimalMission>()
                .HasKey(am => new { am.AnimalId, am.MissionId });
        }
    }
}
