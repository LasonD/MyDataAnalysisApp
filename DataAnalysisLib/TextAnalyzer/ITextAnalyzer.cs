using System.Threading.Tasks;

namespace DataAnalysisLib.TextAnalyzer
{
    public interface ITextAnalyzer
    {
        Task AnalyzeAsync();

        (char characher, int count) MostFrequent { get; }

        (char characher, int count) LeastFrequent { get; }

        int GetCountOf(char ch);
    }
}
