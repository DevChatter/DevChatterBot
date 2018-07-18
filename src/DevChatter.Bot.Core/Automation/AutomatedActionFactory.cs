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
            switch (gameState)
            {
                case DealNoDealGameState.ChosingStartingBoxes:
                {
                    _chatClient.SendMessage($"everyone has {DealNoDealGame.SECONDS_TO_CHOOSE_BOXES} seconds to choose a box!");
                    return new OneTimeCallBackAction(DealNoDealGame.SECONDS_TO_CHOOSE_BOXES, () => _dealNoDealGame.FinishPickingStartingBoxes(), "FinishPickingStartingBoxes");
                }
                case DealNoDealGameState.PickingBoxes:
                {
                    return new PickBoxesAction(_dealNoDealGame, _chatClient);
                }
                case DealNoDealGameState.MakingADeal:
                {
                    return new OfferDealAction(_dealNoDealGame, _chatClient);
                }
                default:
                {
                    return null;
                }
            }
        }
    }
}
