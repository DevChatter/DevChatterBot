using System;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Automation
{
    public class DelayedMessageAction : IIntervalAction
    {
        private readonly string _message;
        private readonly IChatClient _chatClient;
        private DateTime _nextRunTime;
        public TimeSpan DelayTimeSpan;

        public DelayedMessageAction(int delayInSeconds, string message, IChatClient chatClient, string name)
        {
            DelayTimeSpan = TimeSpan.FromSeconds(delayInSeconds);
            _message = message;
            _chatClient = chatClient;
            Name = name;
            _nextRunTime = DateTime.Now.AddSeconds(delayInSeconds);
        }

        public string Name { get; }
        public bool IsTimeToRun() => DateTime.Now > _nextRunTime;

        public void Invoke()
        {
            _chatClient.SendMessage(_message);
            _nextRunTime = DateTime.MaxValue;
        }
    }
}
