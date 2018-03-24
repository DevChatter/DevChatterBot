using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Events;

namespace DevChatter.Bot.Core.Games.RockPaperScissors
{
    public class RockPaperScissorsGame
    {
        private readonly CurrencyGenerator _currencyGenerator;
        private readonly Dictionary<string, RockPaperScissors> _competitors = new Dictionary<string, RockPaperScissors>();
        private readonly object _gameStartLock = new object();
        private bool _isRunningGame;

        public RockPaperScissorsGame(CurrencyGenerator currencyGenerator)
        {
            _currencyGenerator = currencyGenerator;
        }

        public void JoinMatch(IChatClient chatClient, (string username, RockPaperScissors choice) userChoice)
        {
            if (_competitors.ContainsKey(userChoice.username))
            {
                chatClient.SendMessage($"{userChoice.username} changed from {_competitors[userChoice.username]} in the Rock-Paper-Scissors game to {userChoice.choice}!");
            }
            else
            {
                chatClient.SendMessage($"{userChoice.username} joined in the Rock-Paper-Scissors game with {userChoice.choice}!");
            }
            _competitors[userChoice.username] = userChoice.choice;
        }

        public void PlayMatch(IChatClient chatClient)
        {
            RockPaperScissors botChoice = RockPaperScissors.GetRandomChoice();
            chatClient.SendMessage($"I choose {botChoice}!");
            AnnounceWinners(chatClient, botChoice);
            AdjustTokens(botChoice);

            _competitors.Clear();
        }

        public void AdjustTokens(RockPaperScissors botChoice)
        {
            List<string> winnersList = GetWinnerList(botChoice);
            _currencyGenerator.AddCurrencyTo(winnersList, 50);
        }

        public void AnnounceWinners(IChatClient chatClient, RockPaperScissors botChoice)
        {
            List<string> winnersList = GetWinnerList(botChoice);
            string winners = string.Join(",", winnersList);
            chatClient.SendMessage($"The winners are {winners}!");
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
            lock (_gameStartLock)
            {
                if (!_isRunningGame)
                {
                    chatClient.SendMessage($"{username} wants to play Rock-Paper-Scissors! To join, simply type \"!rps\" in chat.");
                    var rpsUpdate = new RockPaperScissorsUpdate(1, this, chatClient);
                    // TODO: connect this to the interval actions
                    _isRunningGame = true;
                }
            }
        }
    }
}