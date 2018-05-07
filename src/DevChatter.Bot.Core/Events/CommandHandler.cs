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

        public CommandHandler(IRepository repository, ICommandUsageTracker usageTracker,
            IEnumerable<IChatClient> chatClients, CommandList commandMessages)
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

            _usageTracker.PurgeExpiredUserCommandCooldowns(DateTimeOffset.UtcNow);

            List<CommandUsage> previousUsages = _usageTracker.GetByUserDisplayName(userDisplayName);
            if (previousUsages != null && previousUsages.Any() && !e.ChatUser.IsInThisRoleOrHigher(UserRole.Mod))
            {
                if (!previousUsages.Any(x => x.WasUserWarned))
                {
                    chatClient.SendMessage($"Whoa {userDisplayName}! Slow down there cowboy!");
                    previousUsages.ForEach(x => x.WasUserWarned = true);
                }

                return;
            }

            IBotCommand botCommand = _commandMessages.FirstOrDefault(c => c.ShouldExecute(e.CommandWord));
            if (botCommand != null)
            {
                CommandUsage commandUsage = AttemptToRunCommand(e, botCommand, chatClient);
                var commandUsageEntity = new CommandUsageEntity(e.CommandWord, botCommand.GetType().FullName,
                    e.ChatUser.UserId, e.ChatUser.DisplayName, chatClient.GetType().Name);
                _repository.Create(commandUsageEntity);
                _usageTracker.RecordUsage(commandUsage);
            }
        }

        private CommandUsage AttemptToRunCommand(CommandReceivedEventArgs e, IBotCommand botCommand, IChatClient chatClient1)
        {
            try
            {
                if (e.ChatUser.CanRunCommand(botCommand))
                {
                    return botCommand.Process(chatClient1, e);
                }
                chatClient1.SendMessage(
                    $"Sorry, {e.ChatUser.DisplayName}! You don't have permission to use the !{e.CommandWord} command.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            return null;
        }
    }
}
