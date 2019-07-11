using System;
using System.Linq;

namespace DevChatter.Bot.Core.Util.FuzzyMatching
{
    public class DamerauLevenshteinMetric : IMetric<String>
    {
        // https://gist.github.com/wickedshimmy/449595/cb33c2d0369551d1aa5b6ff5e6a802e21ba4ad5c
        public Int32 Distance(String x, String y)
        {
            int len_orig = x.Length;
            int len_diff = y.Length;

            var matrix = new int[len_orig + 1, len_diff + 1];
            for (int i = 0; i <= len_orig; i++)
                matrix[i,0] = i;
            for (int j = 0; j <= len_diff; j++)
                matrix[0,j] = j;

            for (int i = 1; i <= len_orig; i++) {
                for (int j = 1; j <= len_diff; j++) {
                    int cost = y[j - 1] == x[i - 1] ? 0 : 1;
                    var vals = new int[] {
                        matrix[i - 1, j] + 1,
                        matrix[i, j - 1] + 1,
                        matrix[i - 1, j - 1] + cost
                    };
                    matrix[i,j] = vals.Min ();
                    if (i > 1 && j > 1 && x[i - 1] == y[j - 2] && x[i - 2] == y[j - 1])
                        matrix[i,j] = Math.Min (matrix[i,j], matrix[i - 2, j - 2] + cost);
                }
            }
            return matrix[len_orig, len_diff];
        }
    }
}
