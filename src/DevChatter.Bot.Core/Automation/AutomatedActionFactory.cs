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
            if (gameState == DealNoDealGameState.CHOSING_STARTING_BOXES)
            {
                return new ChoseStartingBoxesAction(_dealNoDealGame,_chatClient,new SystemClock());
            }
            if (gameState == DealNoDealGameState.PICKING_BOXES)
            {
                return new PickBoxesAction(_dealNoDealGame, _chatClient, new SystemClock());
            }
            if (gameState == DealNoDealGameState.MAKING_A_DEAL)
            {
                return new OfferDealAction(_dealNoDealGame, _chatClient, new SystemClock());
            }

            return null;
        }

    }
}
