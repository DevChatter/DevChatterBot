using DevChatter.Bot.Core.Data.Model;
using System;
using System.Linq.Expressions;

namespace DevChatter.Bot.Core.Data.Specifications
{
    public class HangmanWordPolicy : DataItemPolicy<HangmanWord>
    {
        protected HangmanWordPolicy(Expression<Func<HangmanWord, bool>> expression)
            : base(expression)
        {
        }

        public static HangmanWordPolicy ByWord(string word)
        {
            return new HangmanWordPolicy(x => x.Word == word);
        }
    }
}
