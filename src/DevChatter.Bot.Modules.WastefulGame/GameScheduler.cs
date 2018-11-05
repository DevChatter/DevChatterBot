using DevChatter.Bot.Core.Automation;

namespace DevChatter.Bot.Modules.WastefulGame
{
    public class GameScheduler
    {
        protected readonly int _intervalInSeconds = 5;
        protected readonly RepeatingCallbackAction _action;
        public bool IsGameRunning { get; set; }
        public bool IsGameOpenForJoining { get; set; }

        public GameScheduler(IAutomatedActionSystem automatedActionSystem)
        {
            _action = new RepeatingCallbackAction(OpenGameIfNeeded, _intervalInSeconds);
            automatedActionSystem.AddAction(_action);
        }

        protected void OpenGameIfNeeded()
        {
            if (IsGameRunning) { return; }

            // Lock game-start

            // Announce game

        }
    }
}
