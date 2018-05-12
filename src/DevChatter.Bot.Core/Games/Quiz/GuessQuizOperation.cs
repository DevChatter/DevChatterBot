using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Events.Args;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Games.Quiz
{
    public class GuessQuizOperation : BaseCommandOperation
    {
        private readonly QuizGame _game;

        public GuessQuizOperation(QuizGame game)
        {
            _game = game;
        }

        public override List<string> OperandWords { get; } = new List<string> { "a", "b", "c", "d" };

        public override string HelpText => "Type \"!quiz a\" to guess choice \"a\" in the quiz. I think you can figure out the rest.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            return _game.UpdateGuess(eventArgs.ChatUser, eventArgs.Arguments.First());
        }
    }
}
