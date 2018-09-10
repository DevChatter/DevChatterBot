using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.BotModules.DuelingModule
{
    public class Duel
    {
        private bool _isStarted = false;
        public string Challenger { get; set; }
        public string Opponent { get; set; }

        public void Start() => _isStarted = true;
    }
}
