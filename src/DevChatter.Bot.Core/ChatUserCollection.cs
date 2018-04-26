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
        private readonly List<ChatUser> _activeChatUsers = new List<ChatUser>();

        public ChatUserCollection(IRepository repository)
        {
            _repository = repository;
        }

        public bool NeedToWatchUser(string displayName)
        {
            // Don't lock in here
            // ReSharper disable once InconsistentlySynchronizedField
            return _activeChatUsers.All(x => x.DisplayName != displayName);
        }


        public void WatchUser(ChatUser userFromDb)
        {
            if (NeedToWatchUser(userFromDb.DisplayName))
            {
                lock (_activeChatUsersLock)
                {
                    if (NeedToWatchUser(userFromDb.DisplayName))
                    {
                        _activeChatUsers.Add(userFromDb);
                    }
                }
            }
        }

        public void UpdateEachChatter(Action<ChatUser> updateToApply) =>
            UpdateSpecificChatters(updateToApply, ChatUserPolicy.All());

        public void UpdateSpecificChatters(Action<ChatUser> updateToApply, ISpecification<ChatUser> filter)
        {
            lock (_activeChatUsersLock)
            {
                List<ChatUser> usersToUpdate = _repository.List(filter);

                foreach (ChatUser chatUser in usersToUpdate)
                {
                    updateToApply(chatUser);
                    var userToUpdate = _activeChatUsers.Single(x => x.DisplayName.EqualsIns(chatUser.DisplayName));
                    updateToApply(userToUpdate);
                }
                // TODO: Change this to do a refresh of the activeChatUsers instead of updating one-by-one

                _repository.Update(usersToUpdate);
            }
        }

        public void StopWatching(string displayName)
        {
            lock (_activeChatUsersLock)
            {
                _activeChatUsers.RemoveAll(x => x.DisplayName == displayName);
            }
        }

        public bool UserHasAtLeast(string username, int tokensToRemove)
        {
            ChatUser chatUser = _activeChatUsers.SingleOrDefault(x => x.DisplayName == username);
            if (chatUser == null)
            {
                chatUser = GetOrCreateChatUser(username, new ChatUser { DisplayName = username });
                WatchUser(chatUser);
            }

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
            if (_activeChatUsers.Any(x => x.DisplayName.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                || _repository.Single(ChatUserPolicy.ByDisplayName(username)) != null)
            {
                return true;
            }

            return false;
        }

        public bool TryGiveCoins(string coinGiver, string coinReceiver, int coinsToGive)
        {
            lock (_activeChatUsersLock)
            {
                if (!UserHasAtLeast(coinGiver, coinsToGive))
                {
                    return false;
                }

                if (!UserExists(coinReceiver))
                {
                    return false;
                }

                ChatUser giver = _activeChatUsers.SingleOrDefault(x => x.DisplayName == coinGiver)
                                 ?? GetOrCreateChatUser(coinGiver);
                ChatUser receiver = _activeChatUsers.SingleOrDefault(x => x.DisplayName == coinReceiver)
                                    ?? GetOrCreateChatUser(coinReceiver);

                giver.Tokens -= coinsToGive;
                receiver.Tokens += coinsToGive;

                return true;
            }
        }
    }
}
