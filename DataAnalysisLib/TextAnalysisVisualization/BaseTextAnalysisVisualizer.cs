using DataAnalysisLib.TextAnalyzer;

namespace DataAnalysisLib.TextAnalysisVisualization
{
    public abstract class BaseTextAnalysisVisualizer<TResult>
    {
        protected ITextAnalyzer analyzer;

        protected BaseTextAnalysisVisualizer(ITextAnalyzer analyzer)
        {
            this.analyzer = analyzer;
        }

        public abstract TResult Plot();
    }
}
