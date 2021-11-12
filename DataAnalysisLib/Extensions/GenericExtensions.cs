using System;

namespace DataAnalysisLib.Extensions
{
    public static class GenericExtensions
    {
        public static TOut As<TIn, TOut>(this TIn input, Func<TIn, TOut> func) => func(input);
    }
}
