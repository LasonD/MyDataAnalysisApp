using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataAnalysisLib.TextAnalyzer;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace DataAnalysisLib.TextAnalysisVisualization
{
    public class ExcelTableTextAnalysisVisualizer : BaseTextAnalysisVisualizer
    {
        public ExcelTableTextAnalysisVisualizer(FileInfo outputFile, ITextAnalyzer analyzer) : base(outputFile, analyzer)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public string ReportName { get; set; } = "Text Analysis Report";

        public override async Task PlotAsync()
        {
            await EnsureAnalyzedAsync();
            await CreateAndSaveReport();
        }

        private Task CreateAndSaveReport()
        {
            using var package = new ExcelPackage();

            var workSheet = package.Workbook.Worksheets.Add(ReportName);

            var orderedData = GetOrderedDataSet(out var minIndex, out var maxIndex);

            var range = workSheet.Cells["A1"].LoadFromCollection(orderedData, true);
            range.AutoFitColumns();

            workSheet.Cells[minIndex + 2, 1].AddComment("Min Extrema");
            workSheet.Cells[maxIndex + 2, 1].AddComment("Max Extrema");

            range.Style.Border.BorderAround(ExcelBorderStyle.Medium);

            return package.SaveAsAsync(outputFile);
        }

        private IEnumerable<CharStat> GetOrderedDataSet(out int minIndex, out int maxIndex)
        {
            var result = analyzer.Results.OrderBy(SelectOrderingKey).ToList();

            var min = analyzer.LeastFrequent;
            var max = analyzer.MostFrequent;

            minIndex = result.IndexOf(min);
            maxIndex = result.IndexOf(max);

            return result;
        }

        private object SelectOrderingKey(CharStat charStat) =>
            OrderingCriteria switch
            {
                OrderingCriteria.Char => charStat.Char,
                OrderingCriteria.Occurrences => charStat.Occurrences,
                OrderingCriteria.Frequency => charStat.Frequency,
                _ => throw new System.NotImplementedException(),
            };

        private Task EnsureAnalyzedAsync() => !analyzer.AnalysisComplete ? analyzer.AnalyzeAsync() : Task.CompletedTask;
    }
}
