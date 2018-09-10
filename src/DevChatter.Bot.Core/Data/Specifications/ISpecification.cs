using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        IList<Expression<Func<T, object>>> Includes { get; }
        void AddInclude(Expression<Func<T, object>> expression);
    }
}
