using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Events
{
    public class CommandHandler
    {
        private readonly CommandHandlerSettings _settings;
        private readonly List<IBotCommand> _commandMessages;

        private DateTimeOffset _lastCommandHandledTime = DateTimeOffset.MinValue;

        public CommandHandler(CommandHandlerSettings settings, List<IChatClient> chatClients, List<IBotCommand> commandMessages)
        {
            _settings = settings;
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
                var elapsedSinceLastCommand = DateTimeOffset.Now - _lastCommandHandledTime;
                if (elapsedSinceLastCommand.TotalSeconds < _settings.GlobalCommandCooldown)
                {
                    chatClient.SendMessage("Whoa! Slow down there cowboy!");
                    return;
                }

                IBotCommand botCommand = _commandMessages.FirstOrDefault(c => c.CommandText.ToLowerInvariant() == e.CommandWord.ToLowerInvariant());
                if (botCommand != null)
                {
                    AttemptToRunCommand(e, botCommand, chatClient);
                    _lastCommandHandledTime = DateTimeOffset.Now;
                }
            }
        }

        private void AttemptToRunCommand(CommandReceivedEventArgs e, IBotCommand botCommand, IChatClient chatClient1)
        {
            try
            {
                if (CanUserRunCommand(e.ChatUser, botCommand))
                {
                    botCommand.Process(chatClient1, e);
                }
                else
                {
                    chatClient1.SendMessage(
                        $"Sorry, {e.ChatUser.DisplayName}! You don't have permission to use the !{e.CommandWord} command.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private bool CanUserRunCommand(ChatUser user, IBotCommand botCommand)
        {
            return user.Role <= botCommand.RoleRequired;
        }
    }
}