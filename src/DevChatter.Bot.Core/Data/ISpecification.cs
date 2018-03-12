using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Core.Data
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        string CacheKey { get; }
    }
}