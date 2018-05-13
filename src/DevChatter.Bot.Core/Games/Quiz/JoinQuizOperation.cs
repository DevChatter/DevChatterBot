using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Games.Quiz
{
    internal class JoinQuizOperation : BaseCommandOperation
    {
        private readonly QuizGame _game;

        public JoinQuizOperation(QuizGame game)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public override List<string> OperandWords { get; } = new List<string> {"join", "enter"};

        public override string HelpText => "Type \"!quiz join\" to join the quiz game.";

        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            JoinGameResult joinGameResult = _game.AttemptToJoin(eventArgs.ChatUser);
            return joinGameResult.Message;
        }
    }
}
