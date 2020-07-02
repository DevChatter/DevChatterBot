using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using System.Collections.Generic;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Settings;

namespace DevChatter.Bot.Core.Events
{
    public class CurrencyGenerator : ICurrencyGenerator
    {
        private readonly IChatUserCollection _chatUserCollection;
        private readonly CurrencySettings _currencySettings;

        public CurrencyGenerator(IList<IChatClient> chatClients, IChatUserCollection chatUserCollection, ISettingsFactory settingsFactory)
        {
            _chatUserCollection = chatUserCollection;
            _currencySettings = settingsFactory.GetSettings<CurrencySettings>();
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
            _chatUserCollection.UpdateEachChatter(x => x.Tokens += _currencySettings.CoinsPerInterval);
        }

        public void AddCurrencyTo(IEnumerable<string> listOfNames, int tokensToAdd)
        {
            _chatUserCollection.UpdateSpecificChatters(CappedTokenAdding, ChatUserPolicy.ByDisplayName(listOfNames));

            void CappedTokenAdding(ChatUser chatUser)
            {
                checked
                {
                    chatUser.Tokens += tokensToAdd;
                }
            }
        }

        public void AddCurrencyTo(string displayName, int tokensToAdd)
        {
            AddCurrencyTo(new List<string> { displayName }, tokensToAdd);
        }

        public int RemoveCurrencyFrom(string userName, int tokensToRemove, bool takeAllIfInsufficient = false)
        {
            if (_chatUserCollection.UserHasAtLeast(userName, tokensToRemove))
            {
                _chatUserCollection.UpdateSpecificChatters(x => x.Tokens -= tokensToRemove,
                    ChatUserPolicy.ByDisplayName(userName));
                return tokensToRemove;
            }

            if (takeAllIfInsufficient)
            {
                tokensToRemove = _chatUserCollection.GetOrCreateChatUser(userName).Tokens;
                _chatUserCollection.UpdateSpecificChatters(x => x.Tokens -= tokensToRemove,
                    ChatUserPolicy.ByDisplayName(userName));
                return tokensToRemove;
            }

            return 0;
        }
    }
}
