using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Data
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

        public Expression<Func<T, bool>> Criteria { get; }
        public string CacheKey => $"{typeof(T).Name}-{Criteria}";
    }

    public class CommandPolicy : DataItemPolicy<SimpleCommand>
    {
        protected CommandPolicy(Expression<Func<SimpleCommand, bool>> expression) : base(expression)
        {
        }

        public static CommandPolicy ByCommandText(string commandText)
        {
            return new CommandPolicy(x => x.CommandText == commandText);
        }
    }
}