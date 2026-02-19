using BenchmarkDotNet.Attributes;
using Grondo.Extensions;

namespace Grondo.Benchmarks
{
    [MemoryDiagnoser]
    public class JsonExBenchmarks
    {
        private TestDto _dto = null!;
        private string _json = null!;
        private List<TestDto> _dtoList = null!;
        private string _jsonList = null!;

        [GlobalSetup]
        public void Setup()
        {
            _dto = new TestDto { Id = 42, Name = "Test", Email = "user@example.com" };
            _json = _dto.ToJson();
            _dtoList = [.. Enumerable.Range(0, 100).Select(i => new TestDto { Id = i, Name = $"Item {i}", Email = $"user{i}@example.com" })];
            _jsonList = _dtoList.ToJson();
        }

        [Benchmark]
        public string Serialize_Single() => _dto.ToJson();

        [Benchmark]
        public string Serialize_Single_Indented() => _dto.ToJson(indented: true);

        [Benchmark]
        public string Serialize_List() => _dtoList.ToJson();

        [Benchmark]
        public TestDto? Deserialize_Single() => _json.FromJson<TestDto>();

        [Benchmark]
        public List<TestDto>? Deserialize_List() => _jsonList.FromJson<List<TestDto>>();

        [Benchmark]
        public bool TryFromJson_Valid()
        {
            _json.TryFromJson<TestDto>(out _);
            return true;
        }

        [Benchmark]
        public bool TryFromJson_Invalid()
        {
            "not json".TryFromJson<TestDto>(out _);
            return true;
        }

        public sealed class TestDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }
    }

}
