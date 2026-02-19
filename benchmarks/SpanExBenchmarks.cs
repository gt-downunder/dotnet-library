using BenchmarkDotNet.Attributes;
using Grondo.Extensions;

namespace Grondo.Benchmarks
{
    [MemoryDiagnoser]
    public class SpanExBenchmarks
    {
        private string _haystack = null!;
        private string _needle = null!;
        private string _numericString = null!;
        private string _paddedString = null!;

        [GlobalSetup]
        public void Setup()
        {
            _haystack = "The quick brown fox jumps over the lazy dog";
            _needle = "BROWN FOX";
            _numericString = "12345.6789";
            _paddedString = "   Hello, World!   ";
        }

        [Benchmark]
        public bool ContainsIgnoreCase() => _haystack.AsSpan().ContainsIgnoreCase(_needle.AsSpan());

        [Benchmark]
        public bool IsNumeric_True() => _numericString.AsSpan().IsNumeric();

        [Benchmark]
        public bool IsNumeric_False() => _haystack.AsSpan().IsNumeric();

        [Benchmark]
        public ReadOnlySpan<char> SafeTrim() => _paddedString.AsSpan().SafeTrim();
    }

}
