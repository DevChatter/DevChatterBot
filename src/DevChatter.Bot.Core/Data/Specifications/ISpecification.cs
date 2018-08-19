using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}
