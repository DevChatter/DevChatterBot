using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Automation;

namespace DevChatter.Bot.Core.BotModules.VotingModule
{
    public class DelayableCallbackAction : OneTimeCallBackAction
    {
        public DelayableCallbackAction(int delayInSeconds, Expression<Action> actionToCall)
            : base(delayInSeconds, actionToCall)
        {
        }

        public void SetTimeout(int seconds)
        {
            if (!IsDone)
            {
                _timeOfNextRun = DateTime.UtcNow.AddSeconds(seconds);
            }
        }
    }
}
