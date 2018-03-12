using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Messaging;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Events
{
    public class CommandHandler
    {
        private readonly List<ICommandMessage> _commandMessages;

        public CommandHandler(List<IChatClient> chatClients, List<ICommandMessage> commandMessages)
        {
            _commandMessages = commandMessages;

            foreach (var chatClient in chatClients)
            {
                chatClient.OnCommandReceived += CommandReceivedHandler;
            }
        }

        private void CommandReceivedHandler(object sender, CommandReceivedEventArgs e)
        {
            if (sender is IChatClient chatClient)
            {
                ICommandMessage commandMessage = _commandMessages.FirstOrDefault(c => c.CommandText == e.CommandWord);
                if (commandMessage != null)
                {
                    if (CanUserRunCommand(e.ChatUser, commandMessage))
                    {
                        commandMessage.Process(chatClient, e);
                    }
                    else
                    {
                        chatClient.SendMessage($"Sorry, {e.ChatUser.DisplayName}! You don't have permission to use the !{e.CommandWord} command.");
                    }
                }
            }
        }

        private bool CanUserRunCommand(ChatUser user, ICommandMessage command)
        {
            return user.Role <= command.RoleRequired;
        }
    }
}