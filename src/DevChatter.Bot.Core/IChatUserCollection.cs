using System;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core
{
    public interface IChatUserCollection
    {
        ChatUser GetOrCreateChatUser(string displayName, ChatUser chatUser = null);
        bool NeedToWatchUser(string displayName);
        void StopWatching(string displayName);
        bool TryGiveCoins(string coinGiver, string coinReceiver, int coinsToGive);
        void UpdateEachChatter(Action<ChatUser> updateToApply);
        void UpdateSpecficChatters(Action<ChatUser> updateToApply, Func<ChatUser, bool> filter);
        bool UserExists(string username);
        bool UserHasAtLeast(string username, int tokensToRemove);
        void WatchUser(ChatUser userFromDb);
    }
}
