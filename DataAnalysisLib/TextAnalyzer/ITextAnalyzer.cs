using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAnalysisLib.TextAnalyzer
{
    public interface ITextAnalyzer
    {
        Task AnalyzeAsync();

        CharStat MostFrequent { get; }

        CharStat LeastFrequent { get; }

        IEnumerable<CharStat> Results { get; }

        bool AnalysisComplete { get; }

        int GetCountOf(char ch);
    }
}
