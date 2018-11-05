using DevChatter.Bot.Modules.WastefulGame.Model;
using Microsoft.EntityFrameworkCore;

namespace DevChatter.Bot.Modules.WastefulGame.Data
{
    public class GameDataContext : DbContext
    {
        public DbSet<GameEndRecord> GameEndRecords { get; set; }
        public DbSet<Survivor> Survivors { get; set; }
        public DbSet<Team> Teams { get; set; }

        public GameDataContext()
        {
        }

        public GameDataContext(DbContextOptions<GameDataContext> options)
        : base(options)
        {
        }
    }
}
