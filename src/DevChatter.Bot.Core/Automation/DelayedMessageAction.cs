using System;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Automation
{
    public class DelayedMessageAction
        : IIntervalAction, IAutomatedItem, IDelayed, IAutomatedMessage
    {
        private readonly IChatClient _chatClient;
        private DateTime _nextRunTime;
        public string Name { get; }
        public TimeSpan DelayTimeSpan { get; }
        public string Message { get; }
        private bool _isDone = false;

        public DelayedMessageAction(int delayInSeconds, string message,
            IChatClient chatClient, string name)
        {
            DelayTimeSpan = TimeSpan.FromSeconds(delayInSeconds);
            Message = message;
            _chatClient = chatClient;
            Name = name;
            _nextRunTime = DateTime.Now.AddSeconds(delayInSeconds);
        }

        public bool IsTimeToRun() => DateTime.Now > _nextRunTime;

        public void Invoke()
        {
            _chatClient.SendMessage(Message);
            _nextRunTime = DateTime.MaxValue;
            _isDone = true;
        }

        public bool WillNeverRunAgain()
        {
            return _isDone;
        }
    }
}
