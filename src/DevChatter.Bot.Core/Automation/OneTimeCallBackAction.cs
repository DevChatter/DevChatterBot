using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Core.Automation
{
    public class OneTimeCallBackAction : IIntervalAction
    {
        protected DateTime _timeOfNextRun;
        public Expression<Action> Action { get; }

        public OneTimeCallBackAction(int delayInSeconds, Expression<Action> actionToCall)
        {
            Action = actionToCall;
            _timeOfNextRun = DateTime.UtcNow.AddSeconds(delayInSeconds);
        }

        public bool IsTimeToRun() => DateTime.UtcNow > _timeOfNextRun;

        public void Invoke()
        {
            _timeOfNextRun = DateTime.MaxValue;
            Action.Compile().Invoke();
        }

        public bool IsDone => DateTime.MaxValue == _timeOfNextRun;
    }
}
