using System;
using System.Collections.Generic;
using System.Text;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Games.DealNoDeal;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;


namespace DevChatter.Bot.Core.Automation
{
    public class ChoseStartingBoxesAction : IIntervalAction
    {
        private readonly IClock _clock;
        private DateTime _nextRunTime;
        private IChatClient _chatClient;
        private DealNoDealGame _dealNoDealGame;


        public ChoseStartingBoxesAction(DealNoDealGame dealNoDealGame, IChatClient chatClient, IClock clock)
        {
            _dealNoDealGame = dealNoDealGame;
            _chatClient = chatClient;
            _clock = clock;
            _chatClient.SendMessage($"everyone has {DealNoDealGame.SECONDS_TO_CHOSE_BOXES} seconds to choose a box!");
            SetNextRunTime();
        }

        public bool IsTimeToRun() => _clock.Now >= _nextRunTime;

        public void Invoke()
        {
            _nextRunTime = _clock.Now.AddYears(1);
            _chatClient.SendMessage("Finished picking starting boxes");
            

            _dealNoDealGame.EnsureMinPlayableBoxes();
            _chatClient.SendMessage("Game Started!");
            _dealNoDealGame.GameState = DealNoDealGameState.PICKING_BOXES;
            _dealNoDealGame.SetActionForGameState(DealNoDealGameState.PICKING_BOXES);
        }

        private void SetNextRunTime()
        {
            _nextRunTime = _clock.Now.AddSeconds(DealNoDealGame.SECONDS_TO_CHOSE_BOXES);
        }
    }
}
