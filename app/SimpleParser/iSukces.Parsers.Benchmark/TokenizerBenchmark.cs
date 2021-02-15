using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using iSukces.Parsers.TokenParsers;

namespace iSukces.Parsers.Benchmark
{
    [SimpleJob(RunStrategy.ColdStart, targetCount: 20, invocationCount: 1000_000)]
    public class TokenizerBenchmark
    {
        [Benchmark(Description = "Regexp implementation")]
        public TokenCandidate DoubleTest1()
        {
            return RegexpTokenizer.Parse("      123456.3");
        }

        [Benchmark(Description = "Handmade code")]
        public TokenCandidate Manual()
        {
            return HandMadeTokenizer.Parse("      123456.3");
        }

        private static readonly RegexpDoubleTokenizer RegexpTokenizer 
            = new RegexpDoubleTokenizer(NumerFlags.AllowLedingSpaces);

        private static readonly ManualDoubleTokenizer HandMadeTokenizer =
            new ManualDoubleTokenizer(NumerFlags.AllowLedingSpaces);
    }
}