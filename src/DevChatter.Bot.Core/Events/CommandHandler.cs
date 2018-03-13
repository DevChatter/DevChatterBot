using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Events
{
    public class CommandHandler
    {
        private readonly List<IBotCommand> _commandMessages;

        public CommandHandler(List<IChatClient> chatClients, List<IBotCommand> commandMessages)
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
                IBotCommand botCommand = _commandMessages.FirstOrDefault(c => c.CommandText == e.CommandWord);
                if (botCommand != null)
                {
                    if (CanUserRunCommand(e.ChatUser, botCommand))
                    {
                        botCommand.Process(chatClient, e);
                    }
                    else
                    {
                        chatClient.SendMessage($"Sorry, {e.ChatUser.DisplayName}! You don't have permission to use the !{e.CommandWord} command.");
                    }
                }
            }
        }

        private bool CanUserRunCommand(ChatUser user, IBotCommand botCommand)
        {
            return user.Role <= botCommand.RoleRequired;
        }
    }
}