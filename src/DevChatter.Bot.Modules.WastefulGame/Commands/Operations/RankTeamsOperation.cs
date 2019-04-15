using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Model;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Modules.WastefulGame.Model.Specifications;

namespace DevChatter.Bot.Modules.WastefulGame.Commands.Operations
{
    public class RankTeamsOperation : BaseGameCommandOperation
    {

        private readonly IGameRepository _gameRepository;

        public RankTeamsOperation(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public override List<string> OperandWords { get; }
            = new List<string> { "Rank", "Top", "Ranks" };

        public override string TryToExecute(CommandReceivedEventArgs eventArgs, Survivor survivor)
        {
            List<Team> teams = _gameRepository.List(TeamPolicy.All());

            var teamStats = teams
                .Select(t => new {t.Name, Total = t.Members.Sum(m => m.Money)})
                .OrderByDescending(x => x.Total)
                .Select(x => $"{x.Name}:{x.Total}");

            return $"The wealthiest teams are: {string.Join(", ", teamStats)}.";
        }
    }
}
