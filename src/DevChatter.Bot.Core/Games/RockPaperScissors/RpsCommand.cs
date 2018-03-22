using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Games.RockPaperScissors
{
    public class RockPaperScissorsCommand : SimpleCommand
    {
        private readonly IRepository _repository;

        private readonly Dictionary<string, RockPaperScissors> _competitors = new Dictionary<string, RockPaperScissors>();

        public RockPaperScissorsCommand(IRepository repository)
        {
            _repository = repository;
            CommandText = "rps";
            RoleRequired = UserRole.Everyone;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            string username = eventArgs?.ChatUser?.DisplayName;
            string argumentOne = eventArgs?.Arguments?.ElementAtOrDefault(0);
            if (argumentOne == "start" && username == _competitors.FirstOrDefault().Key)
            {
                PlayMatch(chatClient);
            }
            else
            {
                if (!_competitors.Any())
                {
                    chatClient.SendMessage($"{username} wants to play Rock-Paper-Scissors! To join, simply type \"!rps\" in chat.");
                }

                if (!RockPaperScissors.GetByName(argumentOne, out RockPaperScissors choice))
                {
                    choice = RockPaperScissors.GetRandomChoice();
                    chatClient.SendMessage($"{username} didn't want to pick, so we randomly assigned {choice}!");
                }
                JoinMatch(chatClient, (username, choice));

            }
        }

        private void JoinMatch(IChatClient chatClient, (string username, RockPaperScissors choice) userChoice)
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

        private void PlayMatch(IChatClient chatClient)
        {
            RockPaperScissors botChoice = RockPaperScissors.GetRandomChoice();
            chatClient.SendMessage($"I choose {botChoice}!");
            AnnounceWinners(chatClient, botChoice);
            //AdjustTokens(botChoice);

            _competitors.Clear();
        }

        private void AnnounceWinners(IChatClient chatClient, RockPaperScissors botChoice)
        {
            RockPaperScissors winningChoice = botChoice.LosesTo();
            var winnersList = _competitors.Where(x => x.Value == winningChoice).Select(x => x.Key);
            string winners = string.Join(",", winnersList);
            chatClient.SendMessage($"The winners are {winners}!");
        }

    }
}