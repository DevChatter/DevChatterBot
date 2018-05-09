using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Messaging.Tokens;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class SimpleCommand : DataEntity, IBotCommand
    {
        private DateTimeOffset _timeCommandLastInvoked;
        public TimeSpan Cooldown => TimeSpan.Zero;

        public SimpleCommand()
        {
        }

        public SimpleCommand(string commandText, string staticResponse, UserRole roleRequired = UserRole.Everyone)
        {
            StaticResponse = staticResponse;
            RoleRequired = roleRequired;
            CommandText = commandText;
        }

        public string StaticResponse { get; protected set; }
        public UserRole RoleRequired { get; protected set; }
        public string PrimaryCommandText => CommandText;
        public string CommandText { get; protected set; }
        public string HelpText { get; protected set; } = $"No help text for this command yet.";
        public string FullHelpText => HelpText;

        public bool ShouldExecute(string commandText) => CommandText.EqualsIns(commandText);

        public CommandUsage Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            TimeSpan timePassedSinceInvoke = DateTimeOffset.UtcNow - _timeCommandLastInvoked;
            bool userCanBypassCooldown = eventArgs.ChatUser.Role?.EqualsAny(UserRole.Streamer, UserRole.Mod) ?? false;
            if (userCanBypassCooldown || timePassedSinceInvoke >= Cooldown)
            {
                _timeCommandLastInvoked = DateTimeOffset.UtcNow;

                IEnumerable<string> findTokens = StaticResponse.FindTokens();
                string textToSend = ReplaceTokens(StaticResponse, findTokens, eventArgs);
                chatClient.SendMessage(textToSend);
            }
            else
            {
                string timeRemaining = (Cooldown - timePassedSinceInvoke).ToExpandingString();
                string cooldownMessage = $"That command is currently on cooldown - Remaining time: {timeRemaining}";
                chatClient.SendDirectMessage(eventArgs.ChatUser.DisplayName, cooldownMessage);
            }
            return new CommandUsage(eventArgs.ChatUser.DisplayName, DateTimeOffset.UtcNow, this);
        }

        private string ReplaceTokens(string textToSend, IEnumerable<string> tokens, CommandReceivedEventArgs eventArgs)
        {
            string newText = textToSend;
            var simpleTokens = SimpleToken.ListAll.Where(x => tokens.Contains(x.ReplacementToken));
            foreach (SimpleToken simpleToken in simpleTokens)
            {
                newText = simpleToken.ReplaceCommandValues(newText, eventArgs);
            }

            return newText;
        }

        public TimeSpan GetCooldownTimeRemaining()
        {
            TimeSpan timePassedSinceInvoke = DateTimeOffset.UtcNow - _timeCommandLastInvoked;
            return (Cooldown - timePassedSinceInvoke);
        }

        public bool IsActiveGame() => false;
    }
}
