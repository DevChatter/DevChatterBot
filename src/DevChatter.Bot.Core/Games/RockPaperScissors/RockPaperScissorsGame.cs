using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Games.RockPaperScissors
{
    public class RockPaperScissorsGame : IGame
    {
        private const int SECONDS_TO_JOIN_GAME = 120;
        private const int TOKENS_FOR_WINNING = 100;
        private const int TOKENS_REQUIRED_TO_PLAY = 30;
        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly IAutomatedActionSystem _automatedActionSystem;
        private readonly ILoggerAdapter<RockPaperScissorsGame> _logger;

        private readonly IDictionary<string, RockPaperScissors> _competitors =
            new Dictionary<string, RockPaperScissors>();

        private readonly object _gameStartLock = new object();

        public bool IsRunning { get; private set; }

        public RockPaperScissorsGame(ICurrencyGenerator currencyGenerator, IAutomatedActionSystem automatedActionSystem, ILoggerAdapter<RockPaperScissorsGame> logger)
        {
            _currencyGenerator = currencyGenerator;
            _automatedActionSystem = automatedActionSystem;
            _logger = logger;
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
                if (_currencyGenerator.RemoveCurrencyFrom(userChoice.username, TOKENS_REQUIRED_TO_PLAY))
                {
                    chatClient.SendMessage(
                        $"{userChoice.username} joined in the Rock-Paper-Scissors game with {userChoice.choice}!");
                }
                else
                {
                    chatClient.SendMessage($"You need more coins to play rock-paper-scissors, {userChoice.username}");
                    return;
                }
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
                _currencyGenerator.AddCurrencyTo(winnersList, TOKENS_FOR_WINNING);
        }

        public void AnnounceWinners(IChatClient chatClient, RockPaperScissors botChoice)
        {
            List<string> winnersList = GetWinnerList(botChoice);
            if (winnersList.Any())
            {
                string winners = string.Join(",", winnersList);
                if (winnersList.Count() > 1)
                    chatClient.SendMessage($"The winners are {winners}! They all win {TOKENS_FOR_WINNING} coins!");
                else
                    chatClient.SendMessage($"The winner is {winners}! {winners} wins {TOKENS_FOR_WINNING} coins!");
            }
            else
            {
                chatClient.SendMessage("Nobody won this time!");
            }
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
            chatClient.SendMessage(
                $"{username} wants to play Rock-Paper-Scissors! You have {SECONDS_TO_JOIN_GAME} seconds to join! To join, simply type \"!rps rock\", \"!rps paper\", or \"!rps scissors\" in chat.");

            var triggerEngGame = new OneTimeCallBackAction(SECONDS_TO_JOIN_GAME, () => PlayMatch(chatClient));
            _automatedActionSystem.AddAction(triggerEngGame);
            var lastWarningMessage = new DelayedMessageAction(SECONDS_TO_JOIN_GAME - 30,
                Messages.LAST_CHANCE_TO_JOIN, chatClient);
            _automatedActionSystem.AddAction(lastWarningMessage);
        }
    }

    class Messages
    {
        public const string LAST_CHANCE_TO_JOIN =
            "Only 30 seconds left to join! Type \"!rps rock\", \"!rps paper\", or \"!rps scissors\"";
    }
}
