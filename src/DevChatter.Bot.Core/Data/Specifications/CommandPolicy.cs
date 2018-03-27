using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Commands;

namespace DevChatter.Bot.Core.Data.Specifications
{
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