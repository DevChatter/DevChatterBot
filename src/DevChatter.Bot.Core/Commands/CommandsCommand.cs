using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DevChatter.Bot.Core.Commands
{
    public class CommandsCommand : BaseCommand
    {
        private readonly IServiceProvider _provider;
        private List<ICommandOperation> _operations;

        public List<ICommandOperation> Operations => _operations ?? (_operations = new List<ICommandOperation>
        {
            new AddCommandOperation(Repository, AllCommands),
            new DeleteCommandOperation(Repository, AllCommands),
            new TopCommandsOperation(Repository)
        });

        public CommandsCommand(IRepository repository, IServiceProvider provider)
            : base(repository, UserRole.Everyone)
        {
            _provider = provider;
        }

        private IList<IBotCommand> _allCommands;
        public IList<IBotCommand> AllCommands
        {
            get { return _allCommands ?? (_allCommands = _provider.GetService<IList<IBotCommand>>()); }
        }

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
            var listOfCommands = AllCommands.Where(eventArgsChatUser.CanRunCommand)
                .Select(x => $"!{x.PrimaryCommandText}").ToList();

            string stringOfCommands = string.Join(", ", listOfCommands);
            chatClient.SendMessage(
                $"These are the commands that {eventArgsChatUser.DisplayName} is allowed to run: ({stringOfCommands})");
        }
    }
}
