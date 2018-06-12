using System;

namespace DevChatter.Bot.Core.Automation
{
    public interface IIntervalAction
    {
        string Name { get; }
        bool IsTimeToRun();
        void Invoke();
    }
}
