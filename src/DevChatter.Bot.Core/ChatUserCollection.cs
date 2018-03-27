using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;

namespace DevChatter.Bot.Core
{
    public class ChatUserCollection
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

        public void UpdateEachChatter(Action<ChatUser> updateToApply) => UpdateSpecficChatters(updateToApply, x => true);

        public void UpdateSpecficChatters(Action<ChatUser> updateToApply, Func<ChatUser, bool> filter)
        {
            lock (_activeChatUsersLock)
            {
                List<ChatUser> usersToUpdate = _activeChatUsers.Where(filter).ToList();
                foreach (ChatUser chatUser in usersToUpdate)
                {
                    updateToApply(chatUser);
                }

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
                chatUser = GetOrCreateChatUser(username, new ChatUser {DisplayName = username});
                WatchUser(chatUser);
            }

            return chatUser.Tokens >= tokensToRemove;
        }

        public ChatUser GetOrCreateChatUser(string displayName, ChatUser chatUser)
        {
            lock (_userCreationLock)
            {
                ChatUser userFromDb = _repository.Single(ChatUserPolicy.ByDisplayName(displayName));
                userFromDb = userFromDb ?? _repository.Create(chatUser);
                return userFromDb;
            }
        }

    }
}