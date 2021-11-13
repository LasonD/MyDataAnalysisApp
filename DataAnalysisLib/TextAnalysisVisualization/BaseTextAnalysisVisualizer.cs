using System.IO;
using System.Threading.Tasks;
using DataAnalysisLib.TextAnalyzer;

namespace DataAnalysisLib.TextAnalysisVisualization
{
    public abstract class BaseTextAnalysisVisualizer
    {
        protected readonly Stream outputStream;
        protected ITextAnalyzer analyzer;

        protected BaseTextAnalysisVisualizer(Stream outputStream,  ITextAnalyzer analyzer)
        {
            this.outputStream = outputStream;
            this.analyzer = analyzer;
        }

        public OrderingCriteria OrderingCriteria { get; set; }

        public abstract Task PlotAsync();
    }
}
