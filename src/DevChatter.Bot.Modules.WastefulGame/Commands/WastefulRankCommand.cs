using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Hubs.Dtos;
using DevChatter.Bot.Modules.WastefulGame.Model;
using DevChatter.Bot.Modules.WastefulGame.Model.Specifications;

namespace DevChatter.Bot.Modules.WastefulGame.Commands
{
    public class WastefulRankCommand : BaseCommand
    {
        private readonly IWastefulDisplayNotification _notification;
        private readonly IGameRepository _gameRepository;

        public WastefulRankCommand(IRepository repository,
            IWastefulDisplayNotification notification, IGameRepository gameRepository)
            : base(repository)
        {
            _notification = notification;
            _gameRepository = gameRepository;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var recordDtos = _gameRepository
                .List<Survivor>()
                .OrderByDescending(x => x.Money)
                .Select(x => new SurvivorRecordDto(x));

            var rankingData = new SurvivorRankingDataDto
            {
                Overall = new List<SurvivorRecordDto>(recordDtos)
            };

            _notification.DisplaySurvivorRankings(rankingData);
        }
    }
}
