using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Modules.WastefulGame.Data;
using DevChatter.Bot.Modules.WastefulGame.Model;
using System.Collections.Generic;

namespace DevChatter.Bot.Modules.WastefulGame.Commands.Operations
{
    public class LeaveTeamOperation : BaseGameCommandOperation
    {
        private readonly IGameRepository _gameRepository;

        public LeaveTeamOperation(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public override List<string> OperandWords { get; }
            = new List<string> { "leave", "exit", "escape", "quit" };

        public override string TryToExecute(CommandReceivedEventArgs eventArgs,
            Survivor survivor)
        {
            Team team = survivor.Team;

            if (team == null)
            {
                return "You are not currently on a team.";
            }

            if (survivor.LeaveTeam())
            {
                _gameRepository.Update(survivor);
                return $"You left the {team.Name} team.";
            }

            return $"Failed to leave the team.";
        }
    }
}
