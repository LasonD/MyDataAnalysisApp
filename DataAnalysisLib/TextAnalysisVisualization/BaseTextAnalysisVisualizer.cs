using System.IO;
using System.Threading.Tasks;
using DataAnalysisLib.TextAnalyzer;

namespace DataAnalysisLib.TextAnalysisVisualization
{
    public abstract class BaseTextAnalysisVisualizer
    {
        protected readonly FileInfo outputFile;
        protected ITextAnalyzer analyzer;

        protected BaseTextAnalysisVisualizer(FileInfo outputFile,  ITextAnalyzer analyzer)
        {
            this.outputFile = outputFile;
            this.analyzer = analyzer;
        }

        public OrderingCriteria OrderingCriteria { get; set; }

        public abstract Task PlotAsync();
    }
}
