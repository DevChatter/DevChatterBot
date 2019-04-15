using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Model;
using DevChatter.Bot.Modules.WastefulGame.Model.Specifications;

namespace DevChatter.Bot.Modules.WastefulGame
{
    public class SurvivorRepo
    {
        private readonly IGameRepository _gameRepository;

        public SurvivorRepo(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public Survivor GetOrCreate(ChatUser chatUser)
        {
            return GetOrCreate(chatUser.DisplayName, chatUser.UserId);
        }

        public Survivor GetOrCreate(string displayName, string userId)
        {
            return Get(userId) ?? Create(displayName, userId);
        }

        private Survivor Get(string userId)
        {
            return _gameRepository.Single(SurvivorPolicy.ByUserId(userId));
        }

        private Survivor Create(string displayName, string userId)
        {
            var survivor = new Survivor(displayName, userId);
            _gameRepository.Create(survivor);
            return survivor;
        }

        public void Save(Survivor survivor)
        {
            _gameRepository.Update(survivor);
        }
    }
}
