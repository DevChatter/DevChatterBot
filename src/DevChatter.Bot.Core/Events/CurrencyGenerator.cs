using System.Collections.Generic;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Events
{
    public class CurrencyGenerator
    {
        private readonly ChatUserCollection _chatUserCollection;

        public CurrencyGenerator(List<IChatClient> chatClients, ChatUserCollection chatUserCollection)
        {
            _chatUserCollection = chatUserCollection;
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
                WatchUserIfNeeded(chatter.DisplayName, chatter);
            }
        }

        private void ChatClientOnOnUserNoticed(object sender, UserStatusEventArgs eventArgs)
        {
            WatchUserIfNeeded(eventArgs.DisplayName, eventArgs.ToChatUser());
        }

        private void WatchUserIfNeeded(string displayName, ChatUser chatUser)
        {
            if (_chatUserCollection.NeedToWatchUser(displayName))
            {
                ChatUser userFromDb = _chatUserCollection.GetOrCreateChatUser(displayName, chatUser);
                _chatUserCollection.WatchUser(userFromDb);
            }
        }


        private void ChatClientOnUserLeft(object sender, UserStatusEventArgs eventArgs)
        {
            _chatUserCollection.GetOrCreateChatUser(eventArgs.DisplayName, eventArgs.ToChatUser());

            _chatUserCollection.StopWatching(eventArgs.DisplayName);
        }

        public void UpdateCurrency()
        {
            _chatUserCollection.UpdateEachChatter(x => x.Tokens += 1);
        }

        public void AddCurrencyTo(List<string> listOfNames, int tokensToAdd)
        {
            _chatUserCollection.UpdateSpecficChatters(x => x.Tokens += tokensToAdd, 
                x => listOfNames.Contains(x.DisplayName));
        }

        public bool RemoveCurrencyFrom(string userName, int tokensToRemove)
        {
            if (!_chatUserCollection.UserHasAtLeast(userName, tokensToRemove))
            {
                return false;
            }

            _chatUserCollection.UpdateSpecficChatters(x => x.Tokens -= tokensToRemove,
                x => x.DisplayName == userName);
            return true;
        }
    }
}