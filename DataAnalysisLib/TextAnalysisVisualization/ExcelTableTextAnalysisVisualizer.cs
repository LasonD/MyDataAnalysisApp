using DataAnalysisLib.TextAnalyzer;
using OfficeOpenXml;

namespace DataAnalysisLib.TextAnalysisVisualization
{
    public class ExcelTableTextAnalysisVisualizer : BaseTextAnalysisVisualizer<ExcelWorksheet>
    {
        public ExcelTableTextAnalysisVisualizer(ITextAnalyzer analyzer) : base(analyzer)
        {
        }

        public string ReportName { get; set; } = "Text Analysis Report";

        public override ExcelWorksheet Plot()
        {
            using var package = new ExcelPackage();

            var workSheet = package.Workbook.Worksheets.Add(ReportName);


        }

        internal class AnalysisRow
        {

        }
    }
}
