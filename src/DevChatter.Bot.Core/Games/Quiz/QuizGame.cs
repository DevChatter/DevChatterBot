using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Games.Quiz
{
    public class QuizGame : IGame
    {
        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly IAutomatedActionSystem _automatedActionSystem;

        public Dictionary<string, char> CurrentPlayers { get; set; } = new Dictionary<string, char>();

        public QuizGame(ICurrencyGenerator currencyGenerator, IAutomatedActionSystem automatedActionSystem)
        {
            _currencyGenerator = currencyGenerator;
            _automatedActionSystem = automatedActionSystem;
        }

        public bool IsRunning { get; private set; }

        private bool _questionAskingStarted = false;
        private DelayedMessageAction _messageHint1;
        private DelayedMessageAction _messageHint2;
        private OneTimeCallBackAction _oneTimeActionEndingQuestion;

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

            chatClient.SendMessage($"Starting the quiz now! Our competitors are: {string.Join(", ", CurrentPlayers.Keys)}");

            QuizQuestion randomQuestion = GetRandomQuestion();

            chatClient.SendMessage(randomQuestion.MainQuestion);
            chatClient.SendMessage(randomQuestion.SetAndReturnLetterString());

            _messageHint1 = new DelayedMessageAction(10, $"Hint 1: {randomQuestion.Hint1}", chatClient);
            _automatedActionSystem.AddAction(_messageHint1);
            _messageHint2 = new DelayedMessageAction(20, $"Hint 2: {randomQuestion.Hint2}", chatClient);
            _automatedActionSystem.AddAction(_messageHint2);
            _oneTimeActionEndingQuestion = new OneTimeCallBackAction(30, () => CompleteQuestion(chatClient, randomQuestion));
            _automatedActionSystem.AddAction(_oneTimeActionEndingQuestion);
        }

        private void CompleteQuestion(IChatClient chatClient, QuizQuestion question)
        {
            chatClient.SendMessage($"The correct answer was... {question.CorrectAnswer}");

            AwardWinners(chatClient, question);

            ResetGame();
        }

        private void AwardWinners(IChatClient chatClient, QuizQuestion question)
        {
            char correctLetter = question.LetterAssignment.Single(x => x.Value == question.CorrectAnswer).Key;
            List<string> winners = CurrentPlayers.Where(x => x.Value == correctLetter).Select(x => x.Key).ToList();
            chatClient.SendMessage($"Congratulations to {string.Join(", ", winners)}");
            _currencyGenerator.AddCurrencyTo(winners, 10);
        }

        private void ResetGame()
        {
            IsRunning = false;
            _questionAskingStarted = false;
            CurrentPlayers.Clear();
            _automatedActionSystem.RemoveAction(_messageHint1);
            _automatedActionSystem.RemoveAction(_messageHint2);
            _automatedActionSystem.RemoveAction(_oneTimeActionEndingQuestion);
        }

        private QuizQuestion GetRandomQuestion()
        {
            return new QuizQuestion
            {
                MainQuestion = "Who is the best C# Twitch Streamer?",
                Hint1 = "We aren't wearing hats...",
                Hint2 = "Brendan is modest enough, wouldn't you say?",
                CorrectAnswer = "DevChatter",
                WrongAnswer1 = "CSharpFritz",
                WrongAnswer2 = "Certainly not any of these other choices Kappa ",
                WrongAnswer3 = "robbiew_yt",
            };
        }

        public JoinGameResult AttemptToJoin(ChatUser chatUser)
        {
            if (CurrentPlayers.Any(x => x.Key.EqualsIns(chatUser.DisplayName)))
            {
                return QuizJoinResults.AlreadyInGameResult(chatUser.DisplayName);
            }

            if (!IsGameJoinable)
            {
                return QuizJoinResults.NotJoinTimeResult(chatUser.DisplayName);
            }

            CurrentPlayers[chatUser.DisplayName] = ' ';
            return QuizJoinResults.SuccessJoinResult(chatUser.DisplayName);
        }

        public string UpdateGuess(ChatUser chatUser, string guess)
        {
            if (CurrentPlayers.ContainsKey(chatUser.DisplayName))
            {
                CurrentPlayers[chatUser.DisplayName] = guess.ToLower().Single();
                return $"You updated your guess to {guess}, {chatUser.DisplayName}.";
            }

            return $"You aren't playing. Stop it, {chatUser.DisplayName}.";
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
