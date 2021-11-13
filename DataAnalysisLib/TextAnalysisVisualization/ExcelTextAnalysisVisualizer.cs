using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataAnalysisLib.TextAnalyzer;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace DataAnalysisLib.TextAnalysisVisualization
{
    public class ExcelTextAnalysisVisualizer : BaseTextAnalysisVisualizer
    {
        private const int ChartHeightPx = 600;

        public ExcelTextAnalysisVisualizer(string outputFilePath, ITextAnalyzer analyzer) : base(
            new FileStream(outputFilePath, FileMode.OpenOrCreate), analyzer)
        {

        }

        public ExcelTextAnalysisVisualizer(Stream outputStream, ITextAnalyzer analyzer) : base(outputStream, analyzer)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public string ReportName { get; set; } = "Text Analysis Report";

        public override async Task PlotAsync()
        {
            await EnsureAnalyzedAsync();
            await CreateAndSaveReport();
        }

        private async Task<Stream> CreateAndSaveReport()
        {
            using var package = new ExcelPackage();

            var workSheet = FillWorksheet(package, out var orderedData);

            AddDistributionChart(workSheet, orderedData);

            await package.SaveAsAsync(outputStream);

            return package.Stream;
        }

        private ExcelWorksheet FillWorksheet(ExcelPackage package, out List<CharStat> orderedData)
        {
            var workSheet = package.Workbook.Worksheets.Add(ReportName);

            orderedData = GetOrderedDataSet(out var minIndex, out var maxIndex)
                .ToList();

            var range = workSheet.Cells["A1"].LoadFromCollection(orderedData, true);
            range.AutoFitColumns();

            workSheet.Cells[minIndex + 2, 1].AddComment("Min Extrema");
            workSheet.Cells[maxIndex + 2, 1].AddComment("Max Extrema");

            range.Style.Border.BorderAround(ExcelBorderStyle.Medium);
            return workSheet;
        }

        private static void AddDistributionChart(ExcelWorksheet workSheet, List<CharStat> orderedData)
        {
            var chart = workSheet.Drawings.AddBarChart("Characters Distribution Chart", OfficeOpenXml.Drawing.Chart.eBarChartType.Column3D);
            chart.Title.Text = "Characters distribution";
            chart.SetPosition(0, 0, 3, 0);
            chart.SetSize(orderedData.Count * 20, ChartHeightPx);
            var ser1 = chart.Series.Add(
                workSheet.Cells[$"C2:C{orderedData.Count + 2}"],
                workSheet.Cells[$"A2:A{orderedData.Count + 2}"]);
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
