using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Extensions;

namespace DevChatter.Bot.Core.BotModules.DuelingModule
{
    public class StartChallengeOperation : BaseCommandOperation
    {
        private readonly DuelingSystem _duelingSystem;
        private readonly IChatUserCollection _chatUserCollection;

        public StartChallengeOperation(DuelingSystem duelingSystem, IChatUserCollection chatUserCollection)
        {
            _duelingSystem = duelingSystem;
            _chatUserCollection = chatUserCollection;
        }

        public override List<string> OperandWords => _chatUserCollection.AllChatUsers.ToList();
        public override string HelpText { get; } = "";
        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            string opponent = eventArgs.Arguments[0].NoAt();
            // check for existing game, if already challenged, accept
            var existingChallenge = _duelingSystem.GetChallenges(eventArgs.ChatUser.DisplayName, opponent);
            if (existingChallenge != null)
            {
                _duelingSystem.Accept(existingChallenge);
                return $"A fight breaks out between {existingChallenge.Challenger} and {existingChallenge.Opponent}";
            }
            else
            {
                _duelingSystem.RequestDuel(eventArgs.ChatUser.DisplayName, opponent);
                return $"{eventArgs.ChatUser.DisplayName} is challenging {opponent.NoAt()} to a duel! Type \"!duel {eventArgs.ChatUser.DisplayName}\" to accept!";
            }
        }
    }
}
