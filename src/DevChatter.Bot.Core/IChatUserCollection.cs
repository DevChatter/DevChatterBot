using System;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;

namespace DevChatter.Bot.Core
{
    public interface IChatUserCollection
    {
        ChatUser GetOrCreateChatUser(string displayName, ChatUser chatUser = null);
        bool NeedToWatchUser(string displayName);
        void StopWatching(string displayName);
        bool TryGiveCoins(string coinGiver, string coinReceiver, int coinsToGive);
        void UpdateEachChatter(Action<ChatUser> updateToApply);
        void UpdateSpecificChatters(Action<ChatUser> updateToApply, ISpecification<ChatUser> filter);
        bool UserExists(string username);
        bool UserHasAtLeast(string username, int tokensToRemove);
        void WatchUser(ChatUser userFromDb);
    }
}
