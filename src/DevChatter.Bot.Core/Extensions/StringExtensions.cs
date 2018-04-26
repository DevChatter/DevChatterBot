using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevChatter.Bot.Core.Extensions
{
    public static class StringExtensions
    {
        public static int? SafeToInt(this string src)
        {
            if (int.TryParse(src, out int number))
            {
                return number;
            }

            return null;
        }


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

        /// <summary>
        /// Case Insensitive Equality Comparison using StringComparison.InvariantCultureIgnoreCase
        /// </summary>
        public static bool EqualsIns(this string a, string b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            return a?.Equals(b, StringComparison.InvariantCultureIgnoreCase) ?? false;
        }
    }
}
