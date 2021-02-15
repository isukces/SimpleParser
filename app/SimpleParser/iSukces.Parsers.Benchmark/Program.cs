using System;
using BenchmarkDotNet.Running;

namespace iSukces.Parsers.Benchmark
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("start");
            var summary = BenchmarkRunner.Run<TokenizerBenchmark>();
            Console.WriteLine("stop");
        }
    }
}