using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Extensions;

namespace DevChatter.Bot.Modules.WastefulGame.Model.Specifications
{
    public class LocationSpecification : GameDataPolicy<Location>
    {
        protected LocationSpecification(Expression<Func<Location, bool>> expression)
            : base(expression)
        {
        }

        public static LocationSpecification ByEscapeType(string escapeType)
        {
            return new LocationSpecification(x => x.EscapeType.EqualsIns(escapeType));
        }
    }
}
