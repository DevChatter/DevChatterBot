using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Core.Automation
{
    public interface IIntervalAction
    {
        string Name { get; }
        bool IsTimeToRun();
        void Invoke();
        bool WillNeverRunAgain { get; }
    }

    public interface IAutomatedItem
    {
        string Name { get; }
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
