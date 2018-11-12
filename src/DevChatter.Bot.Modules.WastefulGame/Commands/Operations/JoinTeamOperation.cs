using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Model;
using DevChatter.Bot.Modules.WastefulGame.Model.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Modules.WastefulGame.Commands.Operations
{
    public class JoinTeamOperation : BaseGameCommandOperation
    {
        public const string NO_TEAM_ERROR = "Please specify a valid team.";
        private readonly IGameRepository _gameRepository;

        public JoinTeamOperation(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public override List<string> OperandWords { get; }
            = new List<string> { "join" };
        public override string TryToExecute(CommandReceivedEventArgs eventArgs,
            Survivor survivor)
        {
            string teamArg = eventArgs.Arguments.ElementAtOrDefault(1);
            if (teamArg == null || !int.TryParse(teamArg, out int teamId))
            {
                return NO_TEAM_ERROR;
            }

            var team = _gameRepository.Single(TeamPolicy.ById(teamId));

            if (team == null)
            {
                return NO_TEAM_ERROR;
            }

            if (team.Join(survivor))
            {
                _gameRepository.Update(team);
                return $"Welcome to the {team.Name} team, {survivor.DisplayName}!";
            }

            return "Failed to join team.";
        }
    }
}
