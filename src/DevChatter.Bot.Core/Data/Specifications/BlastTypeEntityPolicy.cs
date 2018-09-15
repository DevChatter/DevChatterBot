using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class BlastTypeEntityPolicy : DataItemPolicy<BlastTypeEntity>
    {
        protected BlastTypeEntityPolicy(Expression<Func<BlastTypeEntity, bool>> expression) : base(expression)
        {
        }

        public static BlastTypeEntityPolicy ByName(string name)
        {
            return new BlastTypeEntityPolicy(x => x.Name == name);
        }
    }
}
