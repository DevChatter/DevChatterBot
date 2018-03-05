using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Messaging;

namespace DevChatter.Bot.Core.Data
{
    public class ActiveMessagePolicy<T> : ISpecification<T> where T : DataItem
    {
        public ActiveMessagePolicy()
        {
            Criteria = message => message.DataItemStatus == DataItemStatus.Active;
        }

        public Expression<Func<T, bool>> Criteria { get; }
    }
}