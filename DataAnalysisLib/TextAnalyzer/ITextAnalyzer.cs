using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAnalysisLib.TextAnalyzer
{
    public interface ITextAnalyzer
    {
        Task AnalyzeAsync();

        KeyValuePair<char, int> MostFrequent { get; }

        KeyValuePair<char, int> LeastFrequent { get; }

        int GetCountOf(char ch);
    }
}
