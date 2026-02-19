using BenchmarkDotNet.Attributes;
using Grondo.Extensions;

namespace Grondo.Benchmarks
{
    [MemoryDiagnoser]
    public class EnumerableExBenchmarks
    {
        private int[] _items = null!;
        private int[] _otherItems = null!;
        private string?[] _nullableItems = null!;

        [GlobalSetup]
        public void Setup()
        {
            _items = [.. Enumerable.Range(1, 1_000)];
            _otherItems = [.. Enumerable.Range(1_001, 1_000)];
            _nullableItems = [.. Enumerable.Range(1, 500).SelectMany<int, string?>(i => [i.ToString(System.Globalization.CultureInfo.InvariantCulture), null])];
        }

        [Benchmark]
        public bool IsNullOrEmpty() => _items.IsNullOrEmpty();

        [Benchmark]
        public bool IsDeepEqualTo() => _items.IsDeepEqualTo(_items);

        [Benchmark]
        public List<IReadOnlyList<int>> Batch() => [.. _items.Batch(100)];

        [Benchmark]
        public (IReadOnlyList<int>, IReadOnlyList<int>) Partition() => _items.Partition(x => x % 2 == 0);

        [Benchmark]
        public List<string> WhereNotNull() => [.. _nullableItems.WhereNotNull()];

        [Benchmark]
        public List<int> Interleave() => [.. _items.Interleave(_otherItems)];
    }
}

