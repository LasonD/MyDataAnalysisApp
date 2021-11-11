using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DataAnalysisLib.TextAnalyzer
{
    public class TextAnalyzer : ITextAnalyzer
    {
        private const int ZeroCount = 0;
        private readonly TextAnalysisResults results = new TextAnalysisResults();
        private readonly Stream dataSource;

        public TextAnalyzer(Stream source)
        {
            dataSource = source ?? throw new ArgumentNullException(nameof(source));
        }

        public async Task AnalyzeAsync()
        {
            await ProcessDataAsync();

            FindExtrema();

            AnalysisComplete = true;
        }

        public (char characher, int count) MostFrequent => results.MostFrequent;

        public (char characher, int count) LeastFrequent => results.LeastFrequent;

        public bool AnalysisComplete { get; private set; }

        public int GetCountOf(char ch)
        {
            ThrowIfAnalysisNotComplete();

            return results.Distribution.TryGetValue(ch, out var count) ? count : ZeroCount;
        }

        private async Task ProcessDataAsync()
        {
            await foreach (var line in EmitLinesAsync())
            {
                ProcessLine(line);
            }
        }

        private void ProcessLine(string line)
        {
            foreach (var ch in line)
            {
                ProcessCharacter(ch);
            }
        }

        private void ProcessCharacter(char ch)
        {
            if (results.Distribution.ContainsKey(ch))
            {
                results.Distribution[ch]++;
            }
            else
            {
                results.Distribution.Add(ch, ZeroCount);
            }
        }

        private async IAsyncEnumerable<string> EmitLinesAsync()
        {
            using var reader = new StreamReader(dataSource);
            while (!reader.EndOfStream)
            {
                yield return await reader.ReadLineAsync();
            }
        }

        private void FindExtrema()
        {
            var (chMin, countMin) = results.Distribution.OrderBy(x => x.Value).First();
            var (chMax, countMax) = results.Distribution.OrderByDescending(x => x.Value).First();

            results.LeastFrequent = (chMin, countMin);
            results.MostFrequent = (chMax, countMax);
        }

        private void ThrowIfAnalysisNotComplete([CallerMemberName] string caller = "") =>
            throw new InvalidOperationException($"Cannot execute {caller}. The analysis is not complete.");
    }
}
