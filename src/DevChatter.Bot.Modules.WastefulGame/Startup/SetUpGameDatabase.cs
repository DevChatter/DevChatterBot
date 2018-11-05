using DevChatter.Bot.Modules.WastefulGame.Data;
using Microsoft.EntityFrameworkCore;

namespace DevChatter.Bot.Modules.WastefulGame.Startup
{
    public static class SetUpGameDatabase
    {
        public static IGameRepository SetUpRepository(string connectionString)
        {
            DbContextOptions<GameDataContext> options = new DbContextOptionsBuilder<GameDataContext>()
                .UseSqlServer(connectionString)
                .Options;

            var dataContext = new GameDataContext(options);

            EnsureDatabase(dataContext);
            IGameRepository repository = new EfGameRepository(dataContext);
            EnsureInitialData(repository);

            return repository;

        }

        private static void EnsureInitialData(IGameRepository repository)
        {
            throw new System.NotImplementedException();
        }

        private static void EnsureDatabase(GameDataContext dataContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
