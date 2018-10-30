using DevChatter.Bot.Core.Data.Specifications;
using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Core.BotModules.WastefulModule.Model.Specifications
{
    public class SurvivorPolicy : DataItemPolicy<Survivor>
    {
        protected SurvivorPolicy(Expression<Func<Survivor, bool>> expression)
            : base(expression)
        {
        }

        public static SurvivorPolicy ByName(string displayName)
        {
            return new SurvivorPolicy(survivor => survivor.DisplayName == displayName);
        }

        public static SurvivorPolicy ByUserId(string userId)
        {
            return new SurvivorPolicy(survivor => survivor.UserId == userId);
        }
    }
}
