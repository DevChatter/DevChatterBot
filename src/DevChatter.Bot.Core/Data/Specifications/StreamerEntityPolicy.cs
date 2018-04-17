using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class StreamerEntityPolicy : DataItemPolicy<StreamerEntity>
    {
        protected StreamerEntityPolicy(Expression<Func<StreamerEntity, bool>> expression) : base(expression)
        {
        }

        public static StreamerEntityPolicy ByChannel(string channelName)
        {
            return new StreamerEntityPolicy(x => x.ChannelName == channelName);
        }
    }
}
