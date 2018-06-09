using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class CommandSettingsPolicy : DataItemPolicy<CommandSettingsEntity>
    {
        protected CommandSettingsPolicy(Expression<Func<CommandSettingsEntity, bool>> expression) : base(expression)
        {
        }

        public static CommandSettingsPolicy ByCommandName(string commandName)
        {
            return new CommandSettingsPolicy(x => x.CommandNameFull == commandName);
        }

        public static CommandSettingsPolicy ByCommandNameAndKey(string commandName, string key)
        {
            return new CommandSettingsPolicy(x => x.CommandNameFull == commandName && x.Key == key);
        }
    }
}
