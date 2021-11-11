using System.Collections.Generic;

namespace DataAnalysisLib.TextAnalyzer
{
    internal class TextAnalysisResults
    {
        public Dictionary<char, int> Distribution { get; set; }

        public (char ch, int count) MostFrequent { get; set; }

        public (char ch, int count) LeastFrequent { get; set; }
    }
}
