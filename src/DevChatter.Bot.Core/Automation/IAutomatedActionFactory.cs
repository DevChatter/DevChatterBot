using System;
using System.Collections.Generic;
using System.Text;
using DevChatter.Bot.Core.Games.DealNoDeal;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Automation
{
    public interface IAutomatedActionFactory
    {
        IIntervalAction GetIntervalAction(DealNoDealGameState gameState);
    }
}
