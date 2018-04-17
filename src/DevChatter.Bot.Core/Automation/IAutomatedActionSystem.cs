using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Automation
{
    public interface IAutomatedActionSystem
    {
        void AddAction(IIntervalAction actionToAdd);
        void RemoveAction(IIntervalAction actionToRemove);
        void RunNecessaryActions();
    }
}
