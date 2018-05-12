using System.Collections.Generic;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Games.Quiz
{
    public class LeaveQuizOperation : BaseCommandOperation
    {
        private QuizGame _game;

        public LeaveQuizOperation(QuizGame game)
        {
            _game = game;
        }

        public override List<string> OperandWords { get; } = new List<string> {"leave", "forfeit", "exit", "forfiet"};

        public override string HelpText => "Type \"!quiz leave\" to forfeit the quiz game.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            return _game.AttemptToLeave(eventArgs.ChatUser);
        }
    }
}
