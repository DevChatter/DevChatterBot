using System.Collections.Generic;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Events
{
    public class CurrencyGenerator : ICurrencyGenerator
    {
        private readonly IChatUserCollection _chatUserCollection;

        public CurrencyGenerator(IList<IChatClient> chatClients, IChatUserCollection chatUserCollection)
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
            IList<ChatUser> allChatters = chatClient.GetAllChatters();
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
                _chatUserCollection.WatchUser(userFromDb.DisplayName);
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
            _chatUserCollection.UpdateSpecificChatters(x => x.Tokens += tokensToAdd, ChatUserPolicy.ByDisplayName(listOfNames));
        }

        public void AddCurrencyTo(string displayName, int tokensToAdd)
        { 
            AddCurrencyTo(new List<string>() { displayName }, tokensToAdd); 
        }

        public bool RemoveCurrencyFrom(string userName, int tokensToRemove)
        {
            if (!_chatUserCollection.UserHasAtLeast(userName, tokensToRemove))
            {
                return false;
            }

            _chatUserCollection.UpdateSpecificChatters(x => x.Tokens -= tokensToRemove, ChatUserPolicy.ByDisplayName(userName));
            return true;
        }
    }
}
