using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Events
{
    public class CommandHandler
    {
        private readonly CommandHandlerSettings _settings;
        private readonly List<IBotCommand> _commandMessages;

        private readonly List<CommandUsage> _userCommandUsages = new List<CommandUsage>();

        public CommandHandler(CommandHandlerSettings settings, List<IChatClient> chatClients,
            List<IBotCommand> commandMessages)
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

                string userDisplayName = e.ChatUser.DisplayName;
                var previousUsage = _userCommandUsages.SingleOrDefault(x => x.DisplayName.EqualsIns(userDisplayName));
                if (previousUsage != null)
                {
                    if (!previousUsage.WasUserWarned)
                    {
                        chatClient.SendMessage($"Whoa {userDisplayName}! Slow down there cowboy!");
                        previousUsage.WasUserWarned = true;
                    }

                    return;
                }

                IBotCommand botCommand = _commandMessages.FirstOrDefault(c => c.CommandText.EqualsIns(e.CommandWord));
                if (botCommand != null)
                {
                    AttemptToRunCommand(e, botCommand, chatClient);
                    _userCommandUsages.Add(new CommandUsage(userDisplayName, currentTime, false));
                }
            }
        }

        private void AttemptToRunCommand(CommandReceivedEventArgs e, IBotCommand botCommand, IChatClient chatClient1)
        {
            try
            {
                if (e.ChatUser.CanUserRunCommand(botCommand))
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

        private void PurgeExpiredUserCommandCooldowns(DateTimeOffset currentTime)
        {
            var expiredCooldowns = new List<CommandUsage>();

            foreach (var cooldownPair in _userCommandUsages)
            {
                var elapsedTime = currentTime - cooldownPair.TimeInvoked;
                if (elapsedTime.TotalSeconds >= _settings.GlobalCommandCooldown)
                {
                    expiredCooldowns.Add(cooldownPair);
                }
            }

            foreach (var user in expiredCooldowns)
            {
                _userCommandUsages.Remove(user);
            }
        }
    }

    public class CommandUsage
    {
        public CommandUsage(string displayName, DateTimeOffset timeInvoked, bool wasUserWarned)
        {
            DisplayName = displayName;
            TimeInvoked = timeInvoked;
            WasUserWarned = wasUserWarned;
        }

        public string DisplayName { get; set; }
        public DateTimeOffset TimeInvoked { get; set; }
        public bool WasUserWarned { get; set; }
    }

}