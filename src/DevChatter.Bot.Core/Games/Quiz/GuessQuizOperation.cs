using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Games.Quiz
{
    internal class GuessQuizOperation : BaseCommandOperation
    {
        private IRepository _repository;
        private QuizGame _game;

        public GuessQuizOperation(IRepository repository, QuizGame game)
        {
            _repository = repository;
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
