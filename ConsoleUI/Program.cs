using System.IO;
using DataAnalysisLib.TextAnalysisVisualization;
using DataAnalysisLib.TextAnalyzer;

namespace ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var textFilePath = "C:\\Users\\Legion\\Desktop\\Test\\Text.txt";
            var outputFilePath = "C:\\Users\\Legion\\Desktop\\Test\\Result.xls";

            var stream = new FileStream(textFilePath, FileMode.Open);

            var analyzer = new TextAnalyzer(stream);

            var outputFile = new FileInfo(outputFilePath);

            var excelVisualizer = new ExcelTableTextAnalysisVisualizer(outputFile, analyzer);

            excelVisualizer.OrderingCriteria = OrderingCriteria.Occurrences;

            excelVisualizer.PlotAsync().Wait();
        }
    }
}
