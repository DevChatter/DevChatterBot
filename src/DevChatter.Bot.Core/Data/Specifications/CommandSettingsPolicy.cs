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

        public static CommandSettingsPolicy BySettingsName(string settingsName)
        {
            return new CommandSettingsPolicy(x => x.CommandNameFull == settingsName);
        }
    }
}
