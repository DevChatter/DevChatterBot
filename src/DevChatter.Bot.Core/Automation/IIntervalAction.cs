using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Core.Automation
{
    public interface IIntervalAction
    {
        bool IsTimeToRun();
        void Invoke();
        bool IsDone { get; }
    }

    public interface IAutomatedItem
    {
    }

    public interface IAutomatedAction
    {
        Expression<Action> Action { get; }
    }

    public interface IAutomatedMessage
    {
        string Message { get; }
    }

    public interface IInterval
    {
        TimeSpan IntervalTimeSpan { get; }
    }

    public interface IDelayed
    {
        TimeSpan DelayTimeSpan { get; }
    }
}
