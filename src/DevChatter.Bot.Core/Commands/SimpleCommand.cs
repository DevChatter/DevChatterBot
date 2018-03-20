using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Messaging.Tokens;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Commands
{
    public class SimpleCommand : DataItem, IBotCommand
    {
        public SimpleCommand()
        {
        }

        public SimpleCommand(string commandText, string staticResponse, UserRole roleRequired = UserRole.Everyone, DataItemStatus dataItemStatus = DataItemStatus.Active)
        {
            _staticResponse = staticResponse;
            RoleRequired = roleRequired;
            CommandText = commandText;
            DataItemStatus = dataItemStatus;
        }

        protected readonly string _staticResponse;
        public UserRole RoleRequired { get; protected set; }
        public string CommandText { get; protected set;  }
        public virtual void Process(IChatClient triggeringClient, CommandReceivedEventArgs eventArgs)
        {
            IEnumerable<string> findTokens = _staticResponse.FindTokens();
            string textToSend = ReplaceTokens(_staticResponse, findTokens, eventArgs);
            triggeringClient.SendMessage(textToSend);
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