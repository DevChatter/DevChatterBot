using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Modules.WastefulGame.Commands.Operations;
using DevChatter.Bot.Modules.WastefulGame.Data;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Modules.WastefulGame.Commands
{
    public class TeamCommand : BaseCommand
    {
        private readonly SurvivorRepo _survivorRepo;
        private readonly List<IGameCommandOperation> _operations;
        public TeamCommand(IRepository mainRepo, IGameRepository gameRepository,
            SurvivorRepo survivorRepo)
            : base(mainRepo)
        {
            _survivorRepo = survivorRepo;
            _operations = new List<IGameCommandOperation>
            {
                new LeaveTeamOperation(gameRepository),
                new JoinTeamOperation(gameRepository),
                new ListTeamsOperation(gameRepository),
            };
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string firstArg = eventArgs.Arguments.FirstOrDefault();
            var survivor = _survivorRepo.GetOrCreate(eventArgs.ChatUser);


            var operation = _operations.SingleOrDefault(op => op.ShouldExecute(firstArg));

            if (operation != null)
            {
                string messageToSend = operation.TryToExecute(eventArgs, survivor);
                chatClient.SendMessage(messageToSend);
            }
            else
            {
                chatClient.SendMessage(survivor.Team?.Name ?? "No Team");
            }
        }
    }
}
