using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Events
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IRepository _repository;
        private readonly ICommandUsageTracker _usageTracker;
        private readonly IList<IBotCommand> _commandMessages;

        public CommandHandler(IRepository repository, ICommandUsageTracker usageTracker, IEnumerable<IChatClient> chatClients,
            CommandList commandMessages)
        {
            _repository = repository;
            _usageTracker = usageTracker;
            _commandMessages = commandMessages;

            foreach (var chatClient in chatClients)
            {
                chatClient.OnCommandReceived += CommandReceivedHandler;
            }
        }

        public void CommandReceivedHandler(object sender, CommandReceivedEventArgs e)
        {
            if (!(sender is IChatClient chatClient))
            {
                return;
            }

            string userDisplayName = e.ChatUser.DisplayName;

            _usageTracker.PurgeExpiredUserCommandCooldowns(DateTimeOffset.Now);

            var previousUsage = _usageTracker.GetByUserDisplayName(userDisplayName);
            if (previousUsage != null && !e.ChatUser.IsInThisRoleOrHigher(UserRole.Mod))
            {
                if (!previousUsage.WasUserWarned)
                {
                    chatClient.SendMessage($"Whoa {userDisplayName}! Slow down there cowboy!");
                    previousUsage.WasUserWarned = true;
                }

                return;
            }

            IBotCommand botCommand = _commandMessages.FirstOrDefault(c => c.ShouldExecute(e.CommandWord));
            if (botCommand != null)
            {
                AttemptToRunCommand(e, botCommand, chatClient);
                var commandUsageEntity = new CommandUsageEntity(e.CommandWord, botCommand.GetType().FullName,
                    e.ChatUser.UserId, e.ChatUser.DisplayName);
                _repository.Create(commandUsageEntity);
                _usageTracker.RecordUsage(new CommandUsage(userDisplayName, DateTimeOffset.Now, false));
            }
        }

        private void AttemptToRunCommand(CommandReceivedEventArgs e, IBotCommand botCommand, IChatClient chatClient1)
        {
            try
            {
                if (e.ChatUser.CanRunCommand(botCommand))
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
    }
}
