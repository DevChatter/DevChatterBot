using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Messaging;
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

        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        {
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