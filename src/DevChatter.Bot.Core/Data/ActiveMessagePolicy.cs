using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Messaging;

namespace DevChatter.Bot.Core.Data
{
    public class ActiveMessagePolicy : ISpecification<IntervalTriggeredMessage>
    {
        public ActiveMessagePolicy()
        {
            Criteria = message => message.DataItemStatus == DataItemStatus.Active;
        }

        public Expression<Func<IntervalTriggeredMessage, bool>> Criteria { get; }
    }
}