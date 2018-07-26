using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Extensions;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class TimezonePolicy : DataItemPolicy<TimezoneEntity>
    {
        protected TimezonePolicy(Expression<Func<TimezoneEntity, bool>> expression) : base(expression)
        {
        }

        public static TimezonePolicy ByLookup(string lookup)
        {
            return new TimezonePolicy(x => x.LookupString.EqualsIns(lookup));
        }
    }
}