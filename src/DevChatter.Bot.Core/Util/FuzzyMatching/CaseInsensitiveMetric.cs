using System;

namespace DevChatter.Bot.Core.Util.FuzzyMatching
{
    public class CaseInsensitiveMetric : IMetric<String>
    {
        private IMetric<String> Metric { get; }

        public CaseInsensitiveMetric(IMetric<String> metric)
        {
            Metric = metric;
        }

        // https://gist.github.com/wickedshimmy/449595/cb33c2d0369551d1aa5b6ff5e6a802e21ba4ad5c
        public Int32 Distance(String x, String y) =>
            Metric.Distance(x.ToLowerInvariant(), y.ToLowerInvariant());
    }
}
