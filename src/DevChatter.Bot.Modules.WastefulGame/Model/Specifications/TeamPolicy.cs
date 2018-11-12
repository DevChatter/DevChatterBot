using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Modules.WastefulGame.Model.Specifications
{
    public class TeamPolicy : GameDataPolicy<Team>
    {
        protected TeamPolicy(Expression<Func<Team, bool>> expression)
            : base(expression)
        {
        }
    }
}
