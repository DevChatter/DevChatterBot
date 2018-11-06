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
            return Get(chatUser) ?? Create(chatUser);
        }

        private Survivor Get(ChatUser chatUser)
        {
            return _gameRepository.Single(SurvivorPolicy.ByUserId(chatUser.UserId));
        }

        private Survivor Create(ChatUser chatUser)
        {
            var survivor = new Survivor(chatUser.DisplayName, chatUser.UserId);
            _gameRepository.Create(survivor);
            return survivor;
        }
    }
}
