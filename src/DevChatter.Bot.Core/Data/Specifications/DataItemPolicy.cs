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
        public IList<string> IncludeStrings { get; } = new List<string>();

        public void AddInclude(Expression<Func<T, object>> expression)
        {
            Includes.Add(expression);
        }

        /// <summary>
        /// Add an include using a string syntax. Commonly used for nested, which would need a ThenInclude otherwise.
        /// </summary>
        /// <param name="stringInclude">Property name to include. Ex: "Aliases.Arguments"</param>
        protected void AddInclude(string stringInclude)
        {
            IncludeStrings.Add(stringInclude);
        }

    }
}
