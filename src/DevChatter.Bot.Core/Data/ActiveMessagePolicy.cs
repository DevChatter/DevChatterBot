using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Messaging;

namespace DevChatter.Bot.Core.Data
{
    public class ActiveMessagePolicy : ISpecification<IAutomatedMessage>
    {
        public ActiveMessagePolicy()
        {
            Criteria = message => message.DataItemStatus == DataItemStatus.Active;
        }

        public Expression<Func<IAutomatedMessage, bool>> Criteria { get; }
    }
}