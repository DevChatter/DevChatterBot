using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Games.RockPaperScissors;

namespace DevChatter.Bot.Core.BotModules.DuelingModule
{
    public class Duel
    {
        private bool _isStarted = false;
        public string Challenger { get; set; }
        public RockPaperScissors ChallengerChoice { get; set; }
        public string Opponent { get; set; }
        public RockPaperScissors OpponentChoice { get; set; }

        public void Start() => _isStarted = true;

        public void ApplySelection(string fromDisplayName, string message)
        {
            if (fromDisplayName.EqualsIns(Challenger)
            && RockPaperScissors.GetByName(message, out RockPaperScissors choice))
            {
                ChallengerChoice = choice;
            }
        }
    }
}
