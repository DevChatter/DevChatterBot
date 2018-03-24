using System;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Games.RockPaperScissors;

namespace DevChatter.Bot.Core.Automation
{
    public class RockPaperScissorsUpdate : IIntervalAction
    {
        private readonly RockPaperScissorsGame _rockPaperScissorsGame;
        private readonly IChatClient _chatClient;
        private readonly int _intervalInMinutes;
        private DateTime _previousRunTime = DateTime.Now;

        public RockPaperScissorsUpdate(int intervalInMinutes, RockPaperScissorsGame rockPaperScissorsGame,
            IChatClient chatClient)
        {
            _intervalInMinutes = intervalInMinutes;
            _rockPaperScissorsGame = rockPaperScissorsGame;
            _chatClient = chatClient;
        }

        public bool IsTimeToRun() => DateTime.Now > _previousRunTime.AddMinutes(_intervalInMinutes);

        public void Invoke()
        {
            _rockPaperScissorsGame.PlayMatch(_chatClient);
            _previousRunTime = DateTime.Now;
        }
    }
}