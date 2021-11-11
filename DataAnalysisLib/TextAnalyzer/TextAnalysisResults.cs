using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisLib.TextAnalyzer
{
    internal class TextAnalysisResults
    {
        private Dictionary<char, int> distribution { get; } = new Dictionary<char, int>();

        public IReadOnlyDictionary<char, int> Distribution => distribution;

        public long TotalCount { get; private set; }

        public void Add(char ch)
        {
            if (distribution.ContainsKey(ch))
            {
                distribution[ch]++;
            }
            else
            {
                distribution.Add(ch, 1);
            }

            TotalCount++;
        }

        public KeyValuePair<char, int> MostFrequent => Distribution.OrderByDescending(x => x.Value).First();

        public KeyValuePair<char, int> LeastFrequent => Distribution.OrderBy(x => x.Value).First();
    }
}
