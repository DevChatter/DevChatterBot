using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class ChatUserPolicy : DataItemPolicy<ChatUser>
    {
        protected ChatUserPolicy(Expression<Func<ChatUser, bool>> expression) : base(expression)
        {
        }

        public static ChatUserPolicy ByDisplayName(string displayName)
        {
            return new ChatUserPolicy(x => x.DisplayName == displayName);
        }
    }
}
