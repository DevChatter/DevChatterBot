using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.ChatSystems;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Events
{
    public class CurrencyGenerator
    {
        private readonly object _userCreationLock = new object();
        private readonly IRepository _repository;
        private readonly ChatUserCollection _chatUserCollection;

        public CurrencyGenerator(List<IChatClient> chatClients, IRepository repository)
        {
            _repository = repository;
            _chatUserCollection = new ChatUserCollection(repository);
            foreach (IChatClient chatClient in chatClients)
            {
                AddCurrentChatters(chatClient);
                chatClient.OnUserNoticed += ChatClientOnOnUserNoticed;
                chatClient.OnUserLeft += ChatClientOnUserLeft;
            }
        }

        private void AddCurrentChatters(IChatClient chatClient)
        {
            List<ChatUser> allChatters = chatClient.GetAllChatters();
            foreach (ChatUser chatter in allChatters)
            {
                WatchUserIfNeeded(chatter.DisplayName, () => chatter);
            }
        }

        private void ChatClientOnOnUserNoticed(object sender, UserStatusEventArgs eventArgs)
        {
            WatchUserIfNeeded(eventArgs.DisplayName, () => eventArgs.ToChatUser());
        }

        private void WatchUserIfNeeded(string displayName, Func<ChatUser> func)
        {
            if (_chatUserCollection.NeedToWatchUser(displayName))
            {
                ChatUser userFromDb = GetOrCreateChatUser(displayName, func);
                _chatUserCollection.WatchUser(userFromDb);
            }
        }

        private ChatUser GetOrCreateChatUser(string displayName, Func<ChatUser> func)
        {
            lock (_userCreationLock)
            {
                ChatUser userFromDb = _repository.Single(ChatUserPolicy.ByDisplayName(displayName));
                userFromDb = userFromDb ?? _repository.Create(func());
                return userFromDb;
            }
        }

        private void ChatClientOnUserLeft(object sender, UserStatusEventArgs eventArgs)
        {
            GetOrCreateChatUser(eventArgs.DisplayName, () => eventArgs.ToChatUser());

            _chatUserCollection.StopWatching(eventArgs.DisplayName);
        }

        public void UpdateCurrency()
        {
            _chatUserCollection.UpdateEachChatter(x => x.Tokens += 10);
        }

        public void AddCurrencyTo(List<string> listOfNames, int tokensToAdd)
        {
            _chatUserCollection.UpdateSpecficChatters(x => x.Tokens += tokensToAdd, 
                x => listOfNames.Contains(x.DisplayName));
        }
    }
}