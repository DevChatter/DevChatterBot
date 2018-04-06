using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Messaging.Tokens;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class SimpleCommand : DataEntity, IBotCommand
    {
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

        public bool ShouldExecute(string commandText) => CommandText.EqualsIns(commandText);

        public virtual void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            IEnumerable<string> findTokens = StaticResponse.FindTokens();
            string textToSend = ReplaceTokens(StaticResponse, findTokens, eventArgs);
            chatClient.SendMessage(textToSend);
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
    }
}