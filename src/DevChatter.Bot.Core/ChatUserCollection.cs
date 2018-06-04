using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Extensions;

namespace DevChatter.Bot.Core
{
    public class ChatUserCollection : IChatUserCollection
    {
        private readonly IRepository _repository;
        private readonly object _userCreationLock = new object();
        private readonly object _activeChatUsersLock = new object();
        private readonly object _currencyLock = new object();
        private readonly List<string> _activeChatUsers = new List<string>();

        public ChatUserCollection(IRepository repository)
        {
            _repository = repository;
        }

        public bool NeedToWatchUser(string displayName)
        {
            // Don't lock in here
            // ReSharper disable once InconsistentlySynchronizedField
            return _activeChatUsers.All(name => name != displayName);
        }


        public void WatchUser(string displayName)
        {
            if (NeedToWatchUser(displayName))
            {
                lock (_activeChatUsersLock)
                {
                    if (NeedToWatchUser(displayName))
                    {
                        _activeChatUsers.Add(displayName);
                    }
                }
            }
        }

        public void UpdateEachChatter(Action<ChatUser> updateToApply)
        {
            UpdateSpecificChatters(updateToApply, ChatUserPolicy.ByDisplayName(_activeChatUsers));
        }

        public void UpdateSpecificChatters(Action<ChatUser> updateToApply, ISpecification<ChatUser> filter)
        {
            lock (_activeChatUsersLock)
            {
                List<ChatUser> usersToUpdate = _repository.List(filter);

                foreach (ChatUser chatUser in usersToUpdate)
                {
                    updateToApply(chatUser);
                }

                _repository.Update(usersToUpdate);
            }
        }

        public void StopWatching(string displayName)
        {
            if (_activeChatUsers.Contains(displayName))
            {
                lock (_activeChatUsersLock)
                {
                    _activeChatUsers.RemoveAll(x => x == displayName);
                }
            }
        }

        public bool UserHasAtLeast(string username, int tokensToRemove)
        {
            ChatUser chatUser = GetOrCreateChatUser(username);
            WatchUser(chatUser.DisplayName);

            return chatUser.Tokens >= tokensToRemove;
        }

        public ChatUser GetOrCreateChatUser(string displayName, ChatUser chatUser = null)
        {
            lock (_userCreationLock)
            {
                ChatUser userFromDb = _repository.Single(ChatUserPolicy.ByDisplayName(displayName));
                userFromDb = userFromDb ?? _repository.Create(chatUser);
                return userFromDb;
            }
        }

        public bool UserExists(string username)
        {
            return _activeChatUsers.Any(x => x.EqualsIns(username))
                   || _repository.Single(ChatUserPolicy.ByDisplayName(username)) != null;
        }

        public bool TryGiveCoins(string coinGiver, string coinReceiver, int coinsToGive)
        {
            lock (_currencyLock)
            {
                if (!UserHasAtLeast(coinGiver, coinsToGive))
                {
                    return false;
                }

                if (!UserExists(coinReceiver))
                {
                    return false;
                }

                ChatUser giver = GetOrCreateChatUser(coinGiver);
                ChatUser receiver = GetOrCreateChatUser(coinReceiver);

                giver.Tokens -= coinsToGive;
                receiver.Tokens += coinsToGive;

                return true;
            }
        }
    }
}
