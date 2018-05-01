using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Commands
{
    public class CommandsCommand : BaseCommand
    {
        private List<ICommandOperation> _operations;

        public List<ICommandOperation> Operations => _operations ?? (_operations = new List<ICommandOperation>
        {
            new AddCommandOperation(Repository, AllCommands),
            new DeleteCommandOperation(Repository, AllCommands)
        });

        public CommandsCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
        }

        public IList<IBotCommand> AllCommands { get; set; }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string oper = eventArgs?.Arguments?.ElementAtOrDefault(0);

            var operationToUse = Operations.SingleOrDefault(x => x.ShouldExecute(oper));
            if (operationToUse != null)
            {
                string resultMessage = operationToUse.TryToExecute(eventArgs);
                chatClient.SendMessage(resultMessage);
            }
            else
            {
                ShowAllCommands(chatClient, eventArgs?.ChatUser);
            }
        }

        private void ShowAllCommands(IChatClient chatClient, ChatUser eventArgsChatUser)
        {
            var listOfCommands = AllCommands.Where(eventArgsChatUser.CanUserRunCommand)
                .Select(x => $"!{x.PrimaryCommandText}").ToList();

            string stringOfCommands = string.Join(", ", listOfCommands);
            chatClient.SendMessage(
                $"These are the commands that {eventArgsChatUser.DisplayName} is allowed to run: ({stringOfCommands})");
        }
    }
}
