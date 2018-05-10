using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Games.Quiz
{
    public class QuizGame : IGame
    {
        private readonly IRepository _repository;
        private readonly IAutomatedActionSystem _automatedActionSystem;

        public List<string> CurrentPlayerNames { get; set; } = new List<string>();

        public QuizGame(IRepository repository, IAutomatedActionSystem automatedActionSystem)
        {
            _repository = repository;
            _automatedActionSystem = automatedActionSystem;
        }

        public bool IsRunning { get; private set; }

        private bool _questionAskingStarted = false;

        private bool IsGameJoinable => IsRunning && !_questionAskingStarted;

        public void StartGame(IChatClient chatClient)
        {
            if (IsRunning)
            {
                return;
            }

            CreateGameJoinWindow(chatClient);

            IsRunning = true;
        }

        private void CreateGameJoinWindow(IChatClient chatClient)
        {
            var joinWarning = new DelayedMessageAction(30, "You only have 30 seconds left to join the quiz game! Type \"!quiz join\" to join the game!", chatClient);
            _automatedActionSystem.AddAction(joinWarning);

            var startAskingQuestions = new OneTimeCallBackAction(60, () => StartAskingQuestions(chatClient));
            _automatedActionSystem.AddAction(startAskingQuestions);
        }

        private void StartAskingQuestions(IChatClient chatClient)
        {
            // TODO: Clean up the automated actions we created
            _questionAskingStarted = true;

            chatClient.SendMessage($"Starting the quiz now! Our competitors are: {string.Join(", ", CurrentPlayerNames)}");
        }

        public JoinGameResult AttemptToJoin(ChatUser chatUser)
        {
            if (CurrentPlayerNames.Any(x => x.EqualsIns(chatUser.DisplayName)))
            {
                return QuizJoinResults.AlreadyInGameResult(chatUser.DisplayName);
            }

            if (!IsGameJoinable)
            {
                return QuizJoinResults.NotJoinTimeResult(chatUser.DisplayName);
            }

            CurrentPlayerNames.Add(chatUser.DisplayName);
            return QuizJoinResults.SuccessJoinResult(chatUser.DisplayName);
        }
    }

    public static class QuizJoinResults
    {
        public static JoinGameResult SuccessJoinResult(string displayName)
            => new JoinGameResult(true, $"{displayName} joined the game!");
        public static JoinGameResult NotJoinTimeResult(string displayName)
            => new JoinGameResult(false, $"Sorry, {displayName} this is not the time to join!");
        public static JoinGameResult AlreadyInGameResult(string displayName)
            => new JoinGameResult(false, $"You're already in this game, {displayName} and you aren't a multi-tasker.");
    }

}
