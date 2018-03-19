using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Data
{
    public class QuoteEntityPolicy : DataItemPolicy<QuoteEntity>
    {
        protected QuoteEntityPolicy(Expression<Func<QuoteEntity, bool>> expression) : base(expression)
        {
        }

        public static QuoteEntityPolicy ByQuoteId(int quoteId)
        {
            return new QuoteEntityPolicy(x => x.QuoteId == quoteId);
        }
    }
}