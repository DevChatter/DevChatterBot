using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Commands;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class SimpleCommandPolicy : DataItemPolicy<SimpleCommand>
    {
        protected SimpleCommandPolicy(Expression<Func<SimpleCommand, bool>> expression) : base(expression)
        {
        }

        public static SimpleCommandPolicy ByCommandText(string commandText)
        {
            return new SimpleCommandPolicy(x => x.CommandText == commandText);
        }
    }
}
