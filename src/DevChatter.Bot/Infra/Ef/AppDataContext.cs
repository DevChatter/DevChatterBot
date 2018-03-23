using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DevChatter.Bot.Infra.Ef
{
    public class AppDataContext : DbContext
    {
        public DbSet<IntervalMessage> IntervalMessages { get; set; }
        public DbSet<SimpleCommand> SimpleResponseMessages { get; set; }
        public DbSet<QuoteEntity> QuoteEntities { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }

        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


    }
    public class AppDataContextFactory : IDesignTimeDbContextFactory<AppDataContext>
    {
        public AppDataContext CreateDbContext(string[] args)
        {
            string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DevChatterBotDb");

            var optionsBuilder = new DbContextOptionsBuilder<AppDataContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDataContext(optionsBuilder.Options);
        }
    }
}