using System;
using System.Collections.Generic;
using System.Text;
using DevChatter.Bot.Core.Games.DealNoDeal;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Automation
{
    public class PickBoxesAction : IIntervalAction
    {
        private readonly IClock _clock;
        private DateTime _nextRunTime;
        private IChatClient _chatClient;
        private DealNoDealGame _dealNoDealGame;


        public PickBoxesAction(DealNoDealGame dealNoDealGame, IChatClient chatClient)
        {
            _dealNoDealGame = dealNoDealGame;
            _chatClient = chatClient;
            _clock = new SystemClock();
            _dealNoDealGame.PrintBoxesRemaining();
            SetNextRunTime();
        }

        public string Name { get; } = nameof(PickBoxesAction);
        public bool IsTimeToRun() => _clock.Now >= _nextRunTime;

        public void Invoke()
        {
                _chatClient.SendMessage("Picking a random Box. User did not respond");
                _dealNoDealGame.PickRandomBox(_dealNoDealGame.MainPlayer.DisplayName);
        }

        private void SetNextRunTime()
        {
            _nextRunTime = _clock.Now.AddSeconds(DealNoDealGame.SECONDS_TO_CHOOSE_BOXES);
        }
    }
}
