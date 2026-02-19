using BenchmarkDotNet.Attributes;
using Grondo.Extensions;

namespace Grondo.Benchmarks
{
    [MemoryDiagnoser]
    public class StringExBenchmarks
    {
        private string _shortString = null!;
        private string _longString = null!;
        private string _email = null!;
        private string _base64 = null!;
        private string _slugInput = null!;
        private string _sensitiveData = null!;

        [GlobalSetup]
        public void Setup()
        {
            _shortString = "Hello, World!";
            _longString = new string('x', 10_000);
            _email = "user@example.com";
            _base64 = Convert.ToBase64String("Hello, World!"u8);
            _slugInput = "Hello World! This is a Test 123";
            _sensitiveData = "4111111111111111";
        }

        [Benchmark]
        public string RemoveSpecialCharacters() => _shortString.RemoveSpecialCharacters();

        [Benchmark]
        public bool IsWellFormedEmailAddress() => _email.IsWellFormedEmailAddress();

        [Benchmark]
        public bool EqualsIgnoreCase() => _shortString.EqualsIgnoreCase("hello, world!");

        [Benchmark]
        public string SafeTrim() => _longString.SafeTrim();

        [Benchmark]
        public string Truncate_Short() => _shortString.Truncate(5);

        [Benchmark]
        public string Truncate_Long() => _longString.Truncate(100);

        [Benchmark]
        public string ToBase64() => _shortString.ToBase64();

        [Benchmark]
        public string FromBase64() => _base64.FromBase64();

        [Benchmark]
        public string Mask() => _sensitiveData.Mask();

        [Benchmark]
        public string ToSlug() => _slugInput.ToSlug();

        [Benchmark]
        public byte[] ToByteArray() => _shortString.ToByteArray();
    }

}
