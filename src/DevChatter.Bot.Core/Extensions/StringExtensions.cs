using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevChatter.Bot.Core.Extensions
{
    public static class StringExtensions
    {
        public static string NoAt(this string src)
        {
            return src.TrimStart('@');
        }

        private static readonly Regex TokenFindingRegex = new Regex(@"\[\w+]");
        public static IEnumerable<string> FindTokens(this string src)
        {
            MatchCollection matches = TokenFindingRegex.Matches(src);
            IEnumerable<Match> matchesEnumerable = matches.OfType<Match>();
            return matchesEnumerable.Select(m => m.Value);
        }
    }
}
