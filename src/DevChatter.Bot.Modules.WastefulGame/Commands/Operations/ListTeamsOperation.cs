using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Model;

namespace DevChatter.Bot.Modules.WastefulGame.Commands.Operations
{
    public class ListTeamsOperation : BaseGameCommandOperation
    {
        private readonly IGameRepository _gameRepository;

        public ListTeamsOperation(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public override List<string> OperandWords { get; }
        = new List<string> { "list", "all", "show" };
        public override string TryToExecute(CommandReceivedEventArgs eventArgs
            , Survivor survivor)
        {
            List<Team> teams = _gameRepository.List<Team>();
            string teamNames = string.Join(", ", teams.Select(team => $"{team.Id}:{team.Name}"));
            return $"The teams are: {teamNames}.";
        }
    }
}
