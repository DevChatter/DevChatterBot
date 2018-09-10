using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class DataItemPolicy<T> : ISpecification<T> where T : DataEntity
    {
        protected DataItemPolicy(Expression<Func<T, bool>> expression)
        {
            Criteria = expression;
        }

        public static DataItemPolicy<T> All()
        {
            return new DataItemPolicy<T>(x => true);
        }

        public static DataItemPolicy<T> ById(Guid id)
        {
            return new DataItemPolicy<T>(x => x.Id == id);
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public IList<Expression<Func<T, object>>> Includes { get; }
            = new List<Expression<Func<T, object>>>();

        public void AddInclude(Expression<Func<T, object>> expression)
            => Includes.Add(expression);

    }
}
