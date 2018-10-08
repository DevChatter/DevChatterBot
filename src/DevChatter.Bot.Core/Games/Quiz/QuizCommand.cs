using System;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Commands.Operations;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Games.Quiz
{
    public class QuizCommand : BaseCommand, IGameCommand
    {
        private List<ICommandOperation> _operations;

        public List<ICommandOperation> Operations => _operations ?? (_operations = new List<ICommandOperation>
        {
            new JoinQuizOperation(_quizGame),
            new LeaveQuizOperation(_quizGame),
            new GuessQuizOperation(_quizGame),
        });

        public QuizCommand(IRepository repository, QuizGame quizGame)
            : base(repository)
        {
            _quizGame = quizGame;
        }

        private readonly QuizGame _quizGame;
        public IGame Game => _quizGame;

        protected override bool HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string argumentOne = eventArgs.Arguments?.FirstOrDefault();

            var operationToUse = Operations.SingleOrDefault(x => x.ShouldExecute(argumentOne));
            if (operationToUse != null)
            {
                string resultMessage = operationToUse.TryToExecute(eventArgs);
                chatClient.SendMessage(resultMessage);
            }
            else
            {
                PerformDefaultTask(eventArgs.ChatUser, chatClient);
            }
        }

        public void PerformDefaultTask(ChatUser chatUser, IChatClient chatClient)
        {
            if (_quizGame.IsRunning)
            {
                chatClient.SendMessage("A QuizGame is already being played.");
            }
            else if (chatUser.IsInThisRoleOrHigher(UserRole.Subscriber))
            {
                _quizGame.StartGame(chatClient);
                chatClient.SendMessage($"{chatUser.DisplayName} is starting a new QuizGame!");
            }
            else
            {
                chatClient.SendMessage($"Sorry, {chatUser.DisplayName}, you must be a subscriber to start a new QuizGame.");
            }
        }
    }
}
