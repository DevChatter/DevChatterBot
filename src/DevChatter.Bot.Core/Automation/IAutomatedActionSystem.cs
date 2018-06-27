using System;

namespace DevChatter.Bot.Core.Automation
{
    public interface IAutomatedActionSystem
    {
        void RunNecessaryActions();
        void AddAction(IIntervalAction actionToAdd);
        void RemoveAction(IIntervalAction actionToRemove);
        void InvokeAction(string id);
    }
}
