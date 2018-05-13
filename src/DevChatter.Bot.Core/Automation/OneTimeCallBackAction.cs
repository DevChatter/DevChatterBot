using System;

namespace DevChatter.Bot.Core.Automation
{
    public class OneTimeCallBackAction : IIntervalAction
    {
        private readonly Action _actionToCall;
        private DateTime _timeOfNextRun;

        public OneTimeCallBackAction(int delayInSeconds, Action actionToCall)
        {
            _actionToCall = actionToCall;
            _timeOfNextRun = DateTime.UtcNow.AddSeconds(delayInSeconds);
        }

        public bool IsTimeToRun() => DateTime.UtcNow > _timeOfNextRun;

        public void Invoke()
        {
            _timeOfNextRun = DateTime.MaxValue;
            _actionToCall();
        }
    }
}
