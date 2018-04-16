using System.Collections.Generic;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Automation
{
    public interface IAutomatedActionSystem
    {
        void AddAutomatedMessages(IEnumerable<IntervalMessage> messages, IList<IChatClient> chatClients);
        void AddAction(IIntervalAction actionToAdd);
        void RemoveAction(IIntervalAction actionToRemove);
        void RunNecessaryActions();
    }
}
