﻿using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisLib.TextAnalyzer
{
    internal class TextAnalysisResults
    {
        private readonly Dictionary<char, CharStat> charStats = new Dictionary<char, CharStat>();

        public IReadOnlyDictionary<char, CharStat> Distribution => charStats;

        public long TotalCount { get; private set; }

        public void Add(char ch)
        {
            CharStat stat;

            if (charStats.ContainsKey(ch))
            {
                stat = charStats[ch];
            }
            else
            {
                stat = new CharStat(ch);

                charStats.Add(ch, stat);
            }

            stat.Occurrences++;
            TotalCount++;
        }

        public void CalculateFrequencies()
        {
            foreach (var stat in charStats.Values)
            {
                stat.CalcFrequency(TotalCount);
            }
        }

        public KeyValuePair<char, CharStat> MostFrequent => Distribution.OrderByDescending(x => x.Value).First();

        public KeyValuePair<char, CharStat> LeastFrequent => Distribution.OrderBy(x => x.Value).First();
    }
}
