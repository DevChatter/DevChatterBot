using System;
using System.Collections.Generic;
using System.Text;
using DevChatter.Bot.Core.Games.DealNoDeal;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Automation
{
    public class OfferDealAction : IIntervalAction
    {
        private readonly IClock _clock;
        private DateTime _nextRunTime;
        private IChatClient _chatClient;
        private DealNoDealGame _dealNoDealGame;


        public OfferDealAction(DealNoDealGame dealNoDealGame, IChatClient chatClient, IClock clock)
        {
            _dealNoDealGame = dealNoDealGame;
            _chatClient = chatClient;
            _clock = clock;
            _dealNoDealGame.PrintBoxesRemaining();
            _chatClient.SendMessage($"***DEAL OFFER: {dealNoDealGame.DealOffer} TOKENS!");
            _chatClient.SendMessage(" type \"!dnd accept\" or \"!dnd decline\"  to accept or decline offer ");
            _chatClient.SendMessage($"You have {DealNoDealGame.SECONDS_TO_CHOOSE_BOXES} seconds or the deal will be automatically accepted");
            SetNextRunTime();
        }

        public bool IsTimeToRun() => _clock.Now >= _nextRunTime;


        public void Invoke()
        {
            _chatClient.SendMessage($"Automatically accepting deal! {DealNoDealGame.SECONDS_TO_CHOOSE_BOXES} seconds passed");
            _dealNoDealGame.AcceptDeal(_chatClient);
        }

        private void SetNextRunTime()
        {
            _nextRunTime = _clock.Now.AddSeconds(DealNoDealGame.SECONDS_TO_CHOOSE_BOXES);
        }
    }
}
