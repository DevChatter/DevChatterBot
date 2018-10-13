using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Systems.Chat;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Messaging
{
    public class IntervalMessageCoordinator
    {
        private readonly IRepository _repository;

        public IntervalMessageCoordinator(IRepository repository)
        {
            _repository = repository;
        }

        public void SendMessage(IChatClient chatClient)
        {
            var allMessages = _repository.List(IntervalMessagePolicy.All());
            IntervalMessage message = SelectMessageToSend(allMessages);

            chatClient.SendMessage(message.MessageText);

            message.LastSent = DateTime.UtcNow;
            _repository.Update(message);
        }

        private IntervalMessage SelectMessageToSend(List<IntervalMessage> allMessages)
        {
            return MyRandom.ChooseRandomWeightedItem(allMessages);
        }
    }
}
