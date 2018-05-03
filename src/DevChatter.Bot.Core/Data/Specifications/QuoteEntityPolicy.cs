using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Data.Model;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class QuoteEntityPolicy : DataItemPolicy<QuoteEntity>
    {
        protected QuoteEntityPolicy(Expression<Func<QuoteEntity, bool>> expression) : base(expression)
        {
        }

        public new static QuoteEntityPolicy All { get; } = new QuoteEntityPolicy(x => true);

        public static QuoteEntityPolicy ByQuoteId(int? quoteId)
        {
            return new QuoteEntityPolicy(x => x.QuoteId == quoteId);
        }
    }
}
