using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class CommandsCommand : BaseCommand
    {
        public CommandsCommand(IRepository repository)
            : base(repository, UserRole.Everyone)
        {
        }

        public IList<IBotCommand> AllCommands { get; set; }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string oper = eventArgs?.Arguments?.ElementAtOrDefault(0);
            string commandWord = eventArgs?.Arguments?.ElementAtOrDefault(1);
            string staticResponse = eventArgs?.Arguments?.ElementAtOrDefault(2);
            string roleRequired = eventArgs?.Arguments?.ElementAtOrDefault(3);

            switch (oper)
            {
                case "add":
                    AddCommand(chatClient, eventArgs?.ChatUser, commandWord, staticResponse, roleRequired);
                    break;
                case "del":
                    DelCommand(chatClient, eventArgs?.ChatUser, commandWord);
                    break;
                default:
                    ShowAllCommands(chatClient, eventArgs?.ChatUser);
                    break;
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

        private void DelCommand(IChatClient chatClient, ChatUser chatUser, string commandWord)
        {
            try
            {
                if (!chatUser.CanUserRunCommand(UserRole.Mod))
                {
                    chatClient.SendMessage("You need to be a moderator to delete a command.");
                    return;
                }

                SimpleCommand command = Repository.Single(CommandPolicy.ByCommandText(commandWord));
                if (command == null)
                {
                    chatClient.SendMessage($"I didn't find a !{commandWord} command.");
                    return;
                }

                chatClient.SendMessage($"Removing the !{commandWord} command.");

                Repository.Remove(command);
                IBotCommand botCommand = AllCommands.SingleOrDefault(x => x.ShouldExecute(commandWord));
                if (botCommand != null)
                {
                    AllCommands.Remove(botCommand);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void AddCommand(IChatClient chatClient, ChatUser chatUser, string commandWord,
            string staticResponse, string roleText)
        {
            try
            {
                if (!chatUser.CanUserRunCommand(UserRole.Mod))
                {
                    chatClient.SendMessage("You need to be a moderator to add a command.");
                    return;
                }

                // !AddCommand Twitter "https://twitter.com/DevChatter_" Everyone
                if (!Enum.TryParse(roleText, true, out UserRole role))
                {
                    role = UserRole.Everyone;
                }

                SimpleCommand command = new SimpleCommand(commandWord, staticResponse, role);

                if (Repository.Single(CommandPolicy.ByCommandText(command.CommandText)) != null)
                {
                    chatClient.SendMessage($"There's already a command using !{command.CommandText}");
                    return;
                }

                chatClient.SendMessage(
                    $"Adding a !{command.CommandText} command for {command.RoleRequired}. It will respond with {command.StaticResponse}.");

                Repository.Create(command);
                AllCommands.Add(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
