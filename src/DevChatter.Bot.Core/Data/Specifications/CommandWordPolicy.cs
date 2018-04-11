using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class CommandWordPolicy : DataItemPolicy<CommandWordEntity>
    {
        protected CommandWordPolicy(Expression<Func<CommandWordEntity, bool>> expression) : base(expression)
        {
        }

        public static CommandWordPolicy OnlyPrimaries()
        {
            return new CommandWordPolicy(x => x.IsPrimary);
        }

        public static CommandWordPolicy ByType(Type type)
        {
            return new CommandWordPolicy(x => x.FullTypeName == type.FullName);
        }

        public static CommandWordPolicy ByWord(string word)
        {
            return new CommandWordPolicy(x => x.CommandWord == word);
        }
    }
}
