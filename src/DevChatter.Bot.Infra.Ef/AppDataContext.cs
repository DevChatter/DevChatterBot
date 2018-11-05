using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DevChatter.Bot.Infra.Ef
{
    public class AppDataContext : DbContext
    {
        public DbSet<IntervalMessage> IntervalMessages { get; set; }
        public DbSet<SimpleCommand> SimpleCommands { get; set; }
        public DbSet<QuoteEntity> QuoteEntities { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<StreamerEntity> Streamers { get; set; }
        public DbSet<CommandEntity> CommandWords { get; set; }
        public DbSet<HangmanWord> HangmanWords { get; set; }
        public DbSet<ScheduleEntity> ScheduleEntities { get; set; }
        public DbSet<CommandUsageEntity> CommandUsages { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<CommandSettingsEntity> CommandSettings { get; set; }
        public DbSet<TimezoneEntity> Timezones { get; set; }
        public DbSet<BlastTypeEntity> BlastTypes { get; set; }
        public DbSet<CanvasProperties> CanvasProperties { get; set; }

        public AppDataContext()
        {
        }

        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString =
                new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["DatabaseConnectionString"];
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuizQuestion>()
                .Ignore(x => x.LetterAssignment);

            modelBuilder.Entity<QuoteEntity>()
                .HasIndex(b => b.QuoteId);

            modelBuilder.Entity<CommandEntity>()
                .HasIndex(b => b.CommandWord).IsUnique();

            modelBuilder.Entity<AliasEntity>()
                .HasIndex(b => b.Word).IsUnique();

            modelBuilder.Entity<BlastTypeEntity>()
                .HasIndex(b => b.Name).IsUnique();

            modelBuilder.Entity<CanvasProperties>()
                .HasIndex(b => b.CanvasId).IsUnique();
        }
    }

    public class AppDataContextFactory : IDesignTimeDbContextFactory<AppDataContext>
    {
        public AppDataContext CreateDbContext(string[] args)
        {
            string connectionString =
                new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["DatabaseConnectionString"];

            var optionsBuilder = new DbContextOptionsBuilder<AppDataContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDataContext(optionsBuilder.Options);
        }
    }
}
