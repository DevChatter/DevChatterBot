using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class IntervalMessagePolicy : DataItemPolicy<IntervalMessage>
    {
        protected IntervalMessagePolicy(Expression<Func<IntervalMessage, bool>> expression)
            : base(expression)
        {
        }
    }
}
