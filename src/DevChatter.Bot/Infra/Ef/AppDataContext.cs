using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
            var optionsBuilder = new DbContextOptionsBuilder<AppDataContext>();
            optionsBuilder.UseSqlServer("Data Source=DevChatterBotDb");

            return new AppDataContext(optionsBuilder.Options);
        }
    }
}