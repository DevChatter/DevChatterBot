using DevChatter.Bot.Modules.WastefulGame.Model;
using DevChatter.Bot.Modules.WastefulGame.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace DevChatter.Bot.Modules.WastefulGame.Data
{
    public class GameDataContext : DbContext
    {
        public DbSet<GameEndRecord> GameEndRecords { get; set; }
        public DbSet<Survivor> Survivors { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<ShopItem> ShopItems { get; set; }
        public DbSet<Location> Locations { get; set; }

        public GameDataContext()
        {
        }

        public GameDataContext(DbContextOptions<GameDataContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["ConnectionStrings:WastefulGame"];
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Location>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder
                .Entity<Location>()
                .Property(x => x.EscapeType)
                .IsRequired();

            modelBuilder
                .Entity<Location>()
                .HasIndex(x => x.EscapeType)
                .IsUnique();

            modelBuilder
                .Entity<ShopItem>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder
                .Entity<GameEndRecord>()
                .Property(x => x.EndType)
                .HasConversion(
                    e => e.ToString(),
                    s => (EndTypes)Enum.Parse(typeof(EndTypes), s));
        }
    }
}
