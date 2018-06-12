using System;

namespace DevChatter.Bot.Core.Automation
{
    public class OneTimeCallBackAction : IIntervalAction
    {
        private readonly Action _actionToCall;
        private DateTime _timeOfNextRun;
        public TimeSpan DelayTimeSpan;

        public OneTimeCallBackAction(int delayInSeconds, Action actionToCall, string name)
        {
            DelayTimeSpan = TimeSpan.FromSeconds(delayInSeconds);
            _actionToCall = actionToCall;
            Name = name;
            _timeOfNextRun = DateTime.UtcNow.AddSeconds(delayInSeconds);
        }

        public string Name { get; }
        public bool IsTimeToRun() => DateTime.UtcNow > _timeOfNextRun;

        public void Invoke()
        {
            _timeOfNextRun = DateTime.MaxValue;
            _actionToCall();
        }
    }
}
