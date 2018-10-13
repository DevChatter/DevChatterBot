using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;
using System;

namespace DevChatter.Bot.Core.Messaging
{
    public class IntervalMessageCoordinator : IIntervalAction
    {
        private readonly IRepository _repository;
        private readonly IChatClient _chatClient;
        private readonly IClock _clock;
        private DateTime _nextRunTime = DateTime.UtcNow.AddMinutes(1);
        private const int INTERVAL_IN_MINUTES = 5;

        public IntervalMessageCoordinator(IRepository repository, IChatClient chatClient, IClock clock)
        {
            _repository = repository;
            _chatClient = chatClient;
            _clock = clock;
        }

        private void SetNextRunTime()
        {
            _nextRunTime = _clock.UtcNow.AddMinutes(INTERVAL_IN_MINUTES);
        }

        public void SendMessage()
        {
            var allMessages = _repository.List(IntervalMessagePolicy.All());
            IntervalMessage message = MyRandom.ChooseRandomWeightedItem(allMessages);

            _chatClient.SendMessage(message.MessageText);

            SetNextRunTime();
            message.LastSent = DateTime.UtcNow;
            _repository.Update(message);
        }

        public bool IsTimeToRun() => _nextRunTime <= _clock.UtcNow;

        public void Invoke() => SendMessage();

        public bool IsDone  => false;
    }
}
