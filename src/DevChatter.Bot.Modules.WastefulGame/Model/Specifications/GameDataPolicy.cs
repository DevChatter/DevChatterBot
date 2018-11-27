using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Specifications;

namespace DevChatter.Bot.Modules.WastefulGame.Model.Specifications
{
    public class GameDataPolicy<T> : ISpecification<T> where T : GameData
    {
        protected GameDataPolicy(Expression<Func<T, bool>> expression)
        {
            Criteria = expression;
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
