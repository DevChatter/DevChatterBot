using System;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Games;
using DevChatter.Bot.Core.Games.RockPaperScissors;

namespace DevChatter.Bot.Core.BotModules.DuelingModule
{
    public class Duel : IGame
    {
        public DateTime DuelExirationTime { get; set; } = DateTime.UtcNow.AddMinutes(1);
        public bool IsRunning { get; private set; }
        public string Challenger { get; set; }
        public RockPaperScissors ChallengerChoice { get; set; }
        public string Opponent { get; set; }
        public RockPaperScissors OpponentChoice { get; set; }

        public void Start() => IsRunning = true;

        public DuelResult ApplySelection(string fromDisplayName, string message)
        {
            if (!RockPaperScissors.GetByName(message, out RockPaperScissors choice))
            {
                return new DuelResult { MessageForUser = "Invalid Choice" };
            }

            if (ChallengerChoice == null && fromDisplayName.EqualsIns(Challenger))
            {
                ChallengerChoice = choice;
            }
            else if (OpponentChoice == null && fromDisplayName.EqualsIns(Opponent))
            {
                OpponentChoice = choice;
            }

            return ContinueIfReady();
        }

        private DuelResult ContinueIfReady()
        {
            if (ChallengerChoice != null && OpponentChoice != null)
            {
                return RunContest();
            }
            return new DuelResult();
        }

        private DuelResult RunContest()
        {
            var result = new DuelResult { DuelIsOver = true };
            if (ChallengerChoice.LosesTo() == OpponentChoice)
            {
                result.MessageForChat = $"In the epic duel, @{Challenger} 's {ChallengerChoice} lost! @{Opponent} 's {OpponentChoice} wins!";
            }
            else if (OpponentChoice.LosesTo() == ChallengerChoice)
            {
                result.MessageForChat = $"In the epic duel, @{Challenger} 's {ChallengerChoice} won! @{Opponent} 's {OpponentChoice} lost!";
            }
            else
            {
                result.MessageForChat = $"The duel between {Challenger} and {Opponent} was a tie! Both picked {ChallengerChoice}.";
            }

            return result;
        }

        public bool IsExpectingInputFrom(string userDisplayName)
        {
            return (Challenger.EqualsIns(userDisplayName) && ChallengerChoice == null)
                   || (Opponent.EqualsIns(userDisplayName) && OpponentChoice == null);
        }

        public bool IsExpired() => DateTime.UtcNow > DuelExirationTime;

        public string GetExpirationMessage()
        {
            return $"The match between {Challenger} and {Opponent} won't happen. Someone was slow.";
        }
    }

    public class DuelResult
    {
        public bool DuelIsOver { get; set; }
        public string MessageForChat { get; set; }
        public string MessageForUser { get; set; }

    }
}
