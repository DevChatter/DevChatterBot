using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Events
{
    public class CurrencyGenerator
    {
        private readonly IRepository _repository;
        private readonly List<ChatUser> _activeChatUsers = new List<ChatUser>();

        public CurrencyGenerator(List<IChatClient> chatClients, IRepository repository)
        {
            _repository = repository;
            foreach (IChatClient chatClient in chatClients)
            {
                chatClient.OnUserNoticed += ChatClientOnOnUserNoticed;
                chatClient.OnUserLeft += ChatClientOnUserLeft;
            }
        }

        private void ChatClientOnOnUserNoticed(object sender, UserStatusEventArgs eventArgs)
        {
            if (_activeChatUsers.All(x => x.DisplayName != eventArgs.DisplayName))
            {
                ChatUser userFromDb = _repository.Single(ChatUserPolicy.ByDisplayName(eventArgs.DisplayName));
                userFromDb = userFromDb ?? _repository.Create(eventArgs.ToChatUser());
                _activeChatUsers.Add(userFromDb);
            }
        }

        private void ChatClientOnUserLeft(object sender, UserStatusEventArgs eventArgs)
        {
            ChatUser userFromDb = _repository.Single(ChatUserPolicy.ByDisplayName(eventArgs.DisplayName));
            if (userFromDb == null)
            {
                _repository.Create(eventArgs.ToChatUser());
            }
            _activeChatUsers.RemoveAll(x => x.DisplayName == eventArgs.DisplayName);
        }

        public void UpdateCurrency()
        {
            foreach (ChatUser activeChatUser in _activeChatUsers)
            {
                activeChatUser.Tokens += 10;
            }

            _repository.Update(_activeChatUsers);
        }
    }
}