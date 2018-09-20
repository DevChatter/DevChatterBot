using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class CommandPolicy : DataItemPolicy<CommandEntity>
    {
        protected CommandPolicy(Expression<Func<CommandEntity, bool>> expression) : base(expression)
        {
            AddInclude(cw => cw.Aliases); // TODO: Include Arguments as well.
        }

        public static CommandPolicy OnlyPrimaries()
        {
            return new CommandPolicy(x => x.IsPrimary);
        }

        public static CommandPolicy ByType(Type type)
        {
            return new CommandPolicy(x => x.FullTypeName == type.FullName);
        }

        public static CommandPolicy ByWord(string word)
        {
            return new CommandPolicy(x => x.CommandWord == word);
        }
    }
}
