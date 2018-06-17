using System;
using System.Collections.Generic;
using System.Text;
using DevChatter.Bot.Core.Games.DealNoDeal;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Automation
{
    public class AutomatedActionFactory : IAutomatedActionFactory
    {
        private readonly DealNoDealGame _dealNoDealGame;
        private readonly IChatClient _chatClient;

        public AutomatedActionFactory(DealNoDealGame dealNoDealGame, IChatClient chatClient)
        {
            _dealNoDealGame = dealNoDealGame;
            _chatClient = chatClient;
        }
        public IIntervalAction GetIntervalAction(DealNoDealGameState gameState)
        {
            if (gameState == DealNoDealGameState.ChosingStartingBoxes)
            {
                return new ChoseStartingBoxesAction(_dealNoDealGame,_chatClient);
            }
            if (gameState == DealNoDealGameState.PickingBoxes)
            {
                return new PickBoxesAction(_dealNoDealGame, _chatClient);
            }
            if (gameState == DealNoDealGameState.MakingADeal)
            {
                return new OfferDealAction(_dealNoDealGame, _chatClient);
            }

            return null;
        }

    }
}
