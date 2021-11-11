using System.Collections.Generic;

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
        }

        public (char ch, int count) MostFrequent { get; set; }

        public (char ch, int count) LeastFrequent { get; set; }
    }
}
