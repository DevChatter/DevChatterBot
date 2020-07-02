using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Games.RockPaperScissors
{
    public class RockPaperScissorsGame : IGame
    {
        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly IAutomatedActionSystem _automatedActionSystem;
        private readonly ILoggerAdapter<RockPaperScissorsGame> _logger;

        private readonly IDictionary<string, RockPaperScissors> _competitors =
            new Dictionary<string, RockPaperScissors>();

        private readonly object _gameStartLock = new object();
        private readonly RockPaperScissorsSettings _settings;

        public bool IsRunning { get; private set; }

        public RockPaperScissorsGame(ICurrencyGenerator currencyGenerator,
            IAutomatedActionSystem automatedActionSystem,
            ISettingsFactory settingsFactory,
            ILoggerAdapter<RockPaperScissorsGame> logger)
        {
            _currencyGenerator = currencyGenerator;
            _automatedActionSystem = automatedActionSystem;
            _logger = logger;
            _settings = settingsFactory.GetSettings<RockPaperScissorsSettings>();
        }

        public void JoinMatch(IChatClient chatClient, (string username, RockPaperScissors choice) userChoice)
        {
            if (_competitors.ContainsKey(userChoice.username))
            {
                chatClient.SendMessage(
                    $"{userChoice.username} changed from {_competitors[userChoice.username]} in the Rock-Paper-Scissors game to {userChoice.choice}!");
            }
            else
            {
                if (_currencyGenerator.RemoveCurrencyFrom(userChoice.username, _settings.TokensRequiredToPlay) == 0)
                {
                    chatClient.SendMessage(Messages.GetShortCoinsMessage(userChoice.username));
                    return;
                }

                chatClient.SendMessage(Messages.GetJoinedMessage(userChoice.username, userChoice.choice));
            }

            _competitors[userChoice.username] = userChoice.choice;
        }

        public void PlayMatch(IChatClient chatClient)
        {
            RockPaperScissors botChoice = RockPaperScissors.GetRandomChoice();
            chatClient.SendMessage($"I choose {botChoice}!");
            AnnounceWinners(chatClient, botChoice);
            AdjustTokens(botChoice);

            CleanUpAfterGame();
        }

        private void CleanUpAfterGame()
        {
            _competitors.Clear();
            IsRunning = false;
        }

        public void AdjustTokens(RockPaperScissors botChoice)
        {
            List<string> winnersList = GetWinnerList(botChoice);
            if (winnersList.Any())
                _currencyGenerator.AddCurrencyTo(winnersList, _settings.TokensForWinning);
        }

        public void AnnounceWinners(IChatClient chatClient, RockPaperScissors botChoice)
        {
            List<string> winnersList = GetWinnerList(botChoice);
            string message = GetWinAnnouncementMessage(winnersList);
            chatClient.SendMessage(message);
        }

        private string GetWinAnnouncementMessage(List<string> winnersList)
        {
            if (!winnersList.Any())
            {
                return "Nobody won this time!";
            }

            if (winnersList.Count > 1)
            {
                string winners = string.Join(", ", winnersList);
                return Messages.GetWinnersMessage(winners, _settings.TokensForWinning);
            }

            return Messages.GetSingleWinnerMessage(winnersList.Single(), _settings.TokensForWinning);
        }

        public List<string> GetWinnerList(RockPaperScissors botChoice)
        {
            RockPaperScissors winningChoice = botChoice.LosesTo();
            return _competitors.Where(x => x.Value == winningChoice).Select(x => x.Key).ToList();
        }

        public bool IsReadyForNewMatch()
        {
            return !_competitors.Any();
        }

        public void AttemptToStartNewGame(IChatClient chatClient, string username)
        {
            if (IsRunning)
            {
                return;
            }

            lock (_gameStartLock)
            {
                if (IsRunning)
                {
                    return;
                }

                IsRunning = true;
            }

            StartNewGame(chatClient, username);
        }

        private void StartNewGame(IChatClient chatClient, string username)
        {
            _logger.LogInformation($"{nameof(StartNewGame)}({chatClient.GetType().Name}, {username}) - Starting Game");
            chatClient.SendMessage(Messages.GetGameStartMessage(username, _settings.SecondsToJoinGame));

            var triggerEngGame = new OneTimeCallBackAction(_settings.SecondsToJoinGame, () => PlayMatch(chatClient));
            _automatedActionSystem.AddAction(triggerEngGame);

            var lastWarningMessage = new DelayedMessageAction(_settings.SecondsToJoinGame - 30,
                Messages.LAST_CHANCE_TO_JOIN, chatClient);
            _automatedActionSystem.AddAction(lastWarningMessage);
        }
    }

    static class Messages
    {
        public const string LAST_CHANCE_TO_JOIN =
            "Only 30 seconds left to join! Type \"!rps rock\", \"!rps paper\", or \"!rps scissors\"";

        public static string GetGameStartMessage(string username, int secondsToJoin) => $"{username} wants to play Rock-Paper-Scissors! You have {secondsToJoin} seconds to join! To join, simply type \"!rps rock\", \"!rps paper\", or \"!rps scissors\" in chat.";

        public static string GetWinnersMessage(string winners, int prize) => $"The winners are {winners}! They all win {prize} coins!";
        public static string GetSingleWinnerMessage(string winner, int prize) => $"The winner is {winner}! {winner} wins {prize} coins!";
        public static string GetChangedMessage(string username, RockPaperScissors choice)
            => throw new NotImplementedException();
        public static string GetJoinedMessage(string username, RockPaperScissors choice)
            => $"{username} joined in the Rock-Paper-Scissors game with {choice}!";
        public static string GetShortCoinsMessage(string username) => $"You need more coins to play rock-paper-scissors, {username}";

    }
}
