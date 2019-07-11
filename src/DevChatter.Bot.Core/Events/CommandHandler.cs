using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Events
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IRepository _repository;
        private readonly ICommandUsageTracker _usageTracker;
        private readonly CommandList _commandList;
        private readonly ILoggerAdapter<CommandHandler> _logger;

        public CommandHandler(IRepository repository, ICommandUsageTracker usageTracker,
            IList<IChatClient> chatClients, CommandList commandList,
            ILoggerAdapter<CommandHandler> logger)
        {
            _repository = repository;
            _usageTracker = usageTracker;
            _commandList = commandList;
            _logger = logger;

            foreach (var chatClient in chatClients)
            {
                chatClient.OnCommandReceived += CommandReceivedHandler;
            }

            SetUpAliasUpdating(commandList);
        }

        private void SetUpAliasUpdating(CommandList commandList)
        {
            var aliasCommand = commandList.GetCommandByType<AliasCommand>();
            if (aliasCommand != null)
            {
                aliasCommand.CommandAliasModified += AliasModified;
            }
        }

        private void AliasModified(object sender, CommandAliasModifiedEventArgs e)
        {
            IBotCommand command = _commandList.GetCommandByFullTypeName(e.FullTypeName);
            if (command is BaseCommand baseCommand)
            {
                baseCommand.NotifyWordsModified();
            }
        }

        public void CommandReceivedHandler(object sender, CommandReceivedEventArgs e)
        {
            if (!(sender is IChatClient chatClient))
            {
                return;
            }

            IList<string> args = new List<string>();
            IBotCommand botCommand = _commandList.FindCommandByKeyword(e.CommandWord, out args);
            if (botCommand == null)
            {
                return;
            }

            var cooldown = _usageTracker.GetActiveCooldown(e.ChatUser, botCommand);

            switch (cooldown)
            {
                case NoCooldown none:
                    ProcessTheCommand(e, chatClient, botCommand, args);
                    break;
                case UserCooldown userCooldown:
                    chatClient.SendDirectMessage(e.ChatUser.DisplayName, userCooldown.Message);
                    break;
                case UserCommandCooldown userCommandCooldown:
                    chatClient.SendDirectMessage(e.ChatUser.DisplayName, userCommandCooldown.Message);
                    break;
                case CommandCooldown commandCooldown:
                    chatClient.SendMessage(commandCooldown.Message);
                    break;
            }
        }

        private void ProcessTheCommand(CommandReceivedEventArgs e,
            IChatClient chatClient, IBotCommand botCommand, IList<string> args)
        {
            CommandUsage commandUsage = AttemptToRunCommand(e, botCommand, chatClient, args);
            var commandUsageEntity = new CommandUsageEntity(e.CommandWord,
                botCommand.GetType().FullName, e.ChatUser.UserId,
                e.ChatUser.DisplayName, chatClient.GetType().Name);
            _repository.Create(commandUsageEntity);
            _usageTracker.RecordUsage(commandUsage);
        }

        private CommandUsage AttemptToRunCommand(CommandReceivedEventArgs e,
            IBotCommand botCommand, IChatClient chatClient1, IList<string> args)
        {
            try
            {
                _logger.LogInformation($"{e.ChatUser.DisplayName} is running the {botCommand.GetType().Name} command.");

                if (e.ChatUser.CanRunCommand(botCommand))
                {
                    if (args.Any())
                    {
                        e.Arguments.Clear();
                        foreach (string arg in args)
                        {
                            e.Arguments.Add(arg);
                        }
                    }
                    return botCommand.Process(chatClient1, e);
                }

                chatClient1.SendMessage(
                    $"Sorry, {e.ChatUser.DisplayName}! You don't have permission to use the !{e.CommandWord} command.");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to run a command.");
            }

            return new CommandUsage(e.ChatUser.DisplayName, DateTimeOffset.UtcNow, botCommand);
        }
    }
}
