using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using iSukces.Parsers.TokenParsers;

namespace iSukces.Parsers.Benchmark
{
    [SimpleJob(RunStrategy.ColdStart, invocationCount: 1000_000)]
    public class TokenizerBenchmark
    {
        [Benchmark(Description = "Number Regexp")]
        public TokenCandidate DoubleTest1()
        {
            return DoubleRegex.Parse("      123456.3");
        }

        [Benchmark(Description = "Number manual")]
        public TokenCandidate Manual()
        {
            return DoubleManual.Parse("      123456.3");
        }

        
        [Benchmark(Description = "Date Regexp")]
        public TokenCandidate DateTimeTest1()
        {
            return DateRegex.Parse("2020-03-02");
        }

        [Benchmark(Description = "Date manual")]
        public TokenCandidate DateTimeManual()
        {
            return DateManual.Parse("2020-03-02");
        }
        
        private static readonly RegexpDoubleTokenizer DoubleRegex 
            = new RegexpDoubleTokenizer(NumerFlags.AllowLedingSpaces);

        private static readonly ManualDoubleTokenizer DoubleManual =
            new ManualDoubleTokenizer(NumerFlags.AllowLedingSpaces);
        
                
        private static readonly ValueTokenizer DateManual = new ManualDateTokenizer(); 
        private static readonly ValueTokenizer DateRegex = new RegexpDateTokenizer();
    }
}