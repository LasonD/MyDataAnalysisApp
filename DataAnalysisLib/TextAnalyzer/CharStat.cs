using System;

namespace DataAnalysisLib.TextAnalyzer
{
    public class CharStat : IEquatable<CharStat>, IComparable<CharStat>
    {
        public CharStat(char ch) => Char = ch; 

        public char Char { get; }

        public double Frequency { get; set; }

        public int Occurrences { get; set; }

        public void CalcFrequency(long total) => Frequency = Occurrences / total;

        public bool Equals(CharStat other) => this.Char == other?.Char;

        // TODO: refine
        public int CompareTo(CharStat other) => this.Occurrences.CompareTo(other.Occurrences);

        public static implicit operator char(CharStat stat) => stat.Char;

        public static implicit operator CharStat(char ch) => new CharStat(ch);
    }
}
