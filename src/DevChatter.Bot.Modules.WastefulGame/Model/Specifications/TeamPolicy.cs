using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Modules.WastefulGame.Model.Specifications
{
    public class TeamPolicy : GameDataPolicy<Team>
    {
        protected TeamPolicy(Expression<Func<Team, bool>> expression)
            : base(expression)
        {
            AddInclude(t => t.Members);
        }

        public static TeamPolicy All()
        {
            return new TeamPolicy(x => true);
        }
        public static TeamPolicy ById(int id)
        {
            return new TeamPolicy(x => x.Id == id);
        }
    }
}
