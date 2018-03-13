using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Messaging.Tokens
{
    public class SimpleToken
    {
        public static SimpleToken UserDisplayName = new SimpleToken(nameof(UserDisplayName), e => e.ChatUser.DisplayName);
        public static SimpleToken CommandWord = new SimpleToken(nameof(CommandWord), e => e.CommandWord);
        public static SimpleToken Arg0 = new SimpleToken(nameof(Arg0), e => e.Arguments?.ElementAtOrDefault(0) ?? "");
        public static SimpleToken Arg1 = new SimpleToken(nameof(Arg1), e => e.Arguments?.ElementAtOrDefault(1) ?? "");
        public static SimpleToken Arg2 = new SimpleToken(nameof(Arg2), e => e.Arguments?.ElementAtOrDefault(2) ?? "");

        protected SimpleToken(string replacementString, Func<CommandReceivedEventArgs, string> replacementValueSelector)
        {
            _replacementString = replacementString;
            _replacementValueSelector = replacementValueSelector;
        }

        protected readonly string _replacementString;
        protected readonly Func<CommandReceivedEventArgs, string> _replacementValueSelector;

        public string ReplacementToken => $"[{_replacementString}]";

        public string ReplaceCommandValues(string inputText, CommandReceivedEventArgs commandReceivedEventArgs)
        {
            return inputText.Replace(ReplacementToken, _replacementValueSelector(commandReceivedEventArgs));
        }

        public static readonly List<SimpleToken> ListAll = new List<SimpleToken>
        {
            UserDisplayName,
            CommandWord,
            Arg0,
            Arg1,
            Arg2,
        };
    }
}