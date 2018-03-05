using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Messaging;

namespace DevChatter.Bot.Core
{
    public class CommandHandler
    {
        private readonly List<StaticCommandResponseMessage> _commandMessages;

        public CommandHandler(List<IChatClient> chatClients, List<StaticCommandResponseMessage> commandMessages)
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
                commandMessage?.Process(chatClient, e);
            }
        }
    }
}