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

        private readonly Dictionary<string, (DateTimeOffset timeInvoked, bool wasUserWarned)> _usersLastCommandHandledTime = new Dictionary<string, (DateTimeOffset timeInvoked, bool wasUserWarned)>();

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
                var currentTime = DateTimeOffset.Now;
                PurgeExpiredUserCommandCooldowns(currentTime);

                if (_usersLastCommandHandledTime.TryGetValue(e.ChatUser.DisplayName, out var lastHandledWarningTuple))
                {
                    if (!lastHandledWarningTuple.wasUserWarned)
                    {
                        chatClient.SendMessage("Whoa! Slow down there cowboy!");
                        _usersLastCommandHandledTime[e.ChatUser.DisplayName] = (lastHandledWarningTuple.timeInvoked, true);
                    }

                    return;
                }

                IBotCommand botCommand = _commandMessages.FirstOrDefault(c => c.CommandText.ToLowerInvariant() == e.CommandWord.ToLowerInvariant());
                if (botCommand != null)
                {
                    AttemptToRunCommand(e, botCommand, chatClient);
                    _usersLastCommandHandledTime.Add(e.ChatUser.DisplayName, (currentTime, false));
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

        private void PurgeExpiredUserCommandCooldowns(DateTimeOffset currentTime)
        {
            var expiredCooldowns = new List<string>();

            foreach (var cooldownPair in _usersLastCommandHandledTime)
            {
                var elapsedTime = currentTime - cooldownPair.Value.timeInvoked;
                if(elapsedTime.TotalSeconds >= _settings.GlobalCommandCooldown)
                    expiredCooldowns.Add(cooldownPair.Key);
            }

            foreach (var user in expiredCooldowns)
            {
                _usersLastCommandHandledTime.Remove(user);
            }
        }
    }
}