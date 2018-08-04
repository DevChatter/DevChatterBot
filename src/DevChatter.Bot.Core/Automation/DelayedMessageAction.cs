using System;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Automation
{
    public class DelayedMessageAction
        : IIntervalAction, IAutomatedItem, IDelayed, IAutomatedMessage
    {
        private readonly IChatClient _chatClient;
        private DateTime _nextRunTime;
        public TimeSpan DelayTimeSpan { get; }
        public string Message { get; }

        public DelayedMessageAction(int delayInSeconds, string message,
            IChatClient chatClient)
        {
            DelayTimeSpan = TimeSpan.FromSeconds(delayInSeconds);
            Message = message;
            _chatClient = chatClient;
            _nextRunTime = DateTime.UtcNow.AddSeconds(delayInSeconds);
        }

        public bool IsTimeToRun() => DateTime.UtcNow > _nextRunTime;

        public void Invoke()
        {
            _chatClient.SendMessage(Message);
            _nextRunTime = DateTime.MaxValue;
        }

        public bool IsDone => DateTime.MaxValue == _nextRunTime;
    }
}
