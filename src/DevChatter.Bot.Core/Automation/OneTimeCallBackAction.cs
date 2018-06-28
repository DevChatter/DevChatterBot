using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Core.Automation
{
    public class OneTimeCallBackAction
        : IIntervalAction, IAutomatedItem, IAutomatedAction, IDelayed
    {
        private DateTime _timeOfNextRun;
        public Expression<Action> Action { get; }
        public TimeSpan DelayTimeSpan { get; }
        public string Name { get; }

        public OneTimeCallBackAction(int delayInSeconds, Expression<Action> actionToCall, string name)
        {
            DelayTimeSpan = TimeSpan.FromSeconds(delayInSeconds);
            Action = actionToCall;
            Name = name;
            _timeOfNextRun = DateTime.UtcNow.AddSeconds(delayInSeconds);
        }

        public bool IsTimeToRun() => DateTime.UtcNow > _timeOfNextRun;

        public void Invoke()
        {
            _timeOfNextRun = DateTime.MaxValue;
            Action.Compile().Invoke();
        }

    }
}
