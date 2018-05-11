using System.Collections.Generic;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Games.Quiz
{
    internal class LeaveQuizOperation : BaseCommandOperation
    {
        private IRepository _repository;
        private QuizGame _game;

        public LeaveQuizOperation(IRepository repository, QuizGame game)
        {
            _repository = repository;
            _game = game;
        }

        public override List<string> OperandWords { get; } = new List<string> {"leave", "forfeit", "exit", "forfiet"};

        public override string HelpText => "Type \"!quiz leave\" to forfeit the quiz game.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            return "You are running the LeaveQuizOperation";
        }
    }
}
