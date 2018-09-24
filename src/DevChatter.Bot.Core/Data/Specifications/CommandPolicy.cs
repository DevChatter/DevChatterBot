using System;
using System.Linq;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class CommandPolicy : DataItemPolicy<CommandEntity>
    {
        protected CommandPolicy(Expression<Func<CommandEntity, bool>> expression) : base(expression)
        {
            AddInclude(cw => cw.Aliases);
            AddInclude($"{nameof(CommandEntity.Aliases)}.{nameof(AliasEntity.Arguments)}");
        }

        public static CommandPolicy ByType(Type type)
        {
            return new CommandPolicy(x => x.FullTypeName == type.FullName && x.IsEnabled);
        }

        public static CommandPolicy ByWord(string word)
        {
            return new CommandPolicy(x => x.CommandWord == word
                                          || x.Aliases.Any(a => a.Word == word));
        }
    }
}
