using FluentAssertions;
using Grondo.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class EnumerableExTests : BaseExtensionTest
    {
        [TestMethod]
        public void IsNullOrEmpty_Null_ReturnsTrue()
        {
            IEnumerable<int>? source = null;
            source.IsNullOrEmpty().Should().BeTrue();
        }

        [TestMethod]
        public void IsNullOrEmpty_Empty_ReturnsTrue() =>
            Array.Empty<int>().IsNullOrEmpty().Should().BeTrue();

        [TestMethod]
        public void IsNullOrEmpty_HasItems_ReturnsFalse() =>
            new[] { 1 }.IsNullOrEmpty().Should().BeFalse();

        [TestMethod]
        public void IsNotNullOrEmpty_Null_ReturnsFalse()
        {
            IEnumerable<int>? source = null;
            source.IsNotNullOrEmpty().Should().BeFalse();
        }

        [TestMethod]
        public void IsNotNullOrEmpty_HasItems_ReturnsTrue() =>
            new[] { 1, 2 }.IsNotNullOrEmpty().Should().BeTrue();

        [TestMethod]
        public void IsEmpty_EmptyCollection_ReturnsTrue() =>
            Array.Empty<string>().IsEmpty().Should().BeTrue();

        [TestMethod]
        public void IsEmpty_NonEmpty_ReturnsFalse() =>
            new[] { "a" }.IsEmpty().Should().BeFalse();

        [TestMethod]
        public void IsEmpty_Null_ThrowsArgumentNullException()
        {
            IEnumerable<int> source = null!;
            Func<bool> act = () => source.IsEmpty();
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void IsNotEmpty_NonEmpty_ReturnsTrue() =>
            new[] { 1 }.IsNotEmpty().Should().BeTrue();

        [TestMethod]
        public void IsDeepEqualTo_EqualSequences_ReturnsTrue() =>
            new[] { 1, 2, 3 }.IsDeepEqualTo([1, 2, 3]).Should().BeTrue();

        [TestMethod]
        public void IsDeepEqualTo_DifferentSequences_ReturnsFalse() =>
            new[] { 1, 2, 3 }.IsDeepEqualTo([3, 2, 1]).Should().BeFalse();

        [TestMethod]
        public void ForEach_Action_ExecutesForEachWithIndex()
        {
            string[] items = ["a", "b", "c"];
            var collected = new List<(string item, int index)>();

            items.ForEach((item, index) => collected.Add((item, index)));

            collected.Should().HaveCount(3);
            collected[0].Should().Be(("a", 0));
            collected[2].Should().Be(("c", 2));
        }

        [TestMethod]
        public void ForEach_Func_ReturnsFalseOnEarlyBreak()
        {
            int[] items = [1, 2, 3, 4, 5];
            var processed = new List<int>();

            bool result = items.ForEach((item, _) =>
            {
                processed.Add(item);
                return item < 3;
            });

            result.Should().BeFalse();
            processed.Should().BeEquivalentTo([1, 2, 3]);
        }

        [TestMethod]
        public void ForEach_Func_ReturnsTrueWhenAllProcessed()
        {
            bool result = new[] { 1, 2, 3 }.ForEach((_, _) => true);
            result.Should().BeTrue();
        }

        // --- Batch ---

        [TestMethod]
        public void Batch_EvenSplit_ReturnsEqualBatches()
        {
            int[] items = [1, 2, 3, 4, 5, 6];
            var batches = items.Batch(3).ToList();

            batches.Should().HaveCount(2);
            batches[0].Should().BeEquivalentTo([1, 2, 3]);
            batches[1].Should().BeEquivalentTo([4, 5, 6]);
        }

        [TestMethod]
        public void Batch_UnevenSplit_LastBatchSmaller()
        {
            int[] items = [1, 2, 3, 4, 5];
            var batches = items.Batch(3).ToList();

            batches.Should().HaveCount(2);
            batches[0].Should().HaveCount(3);
            batches[1].Should().HaveCount(2);
        }

        [TestMethod]
        public void Batch_EmptySource_ReturnsEmpty() => Array.Empty<int>().Batch(5).Should().BeEmpty();

        [TestMethod]
        public void Batch_SizeZero_Throws()
        {
            Action act = () => _ = new[] { 1 }.Batch(0).ToList();
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        // --- Partition ---

        [TestMethod]
        public void Partition_SplitsCorrectly()
        {
            int[] items = [1, 2, 3, 4, 5, 6];
            (IReadOnlyList<int>? matches, IReadOnlyList<int>? nonMatches) = items.Partition(x => x % 2 == 0);

            matches.Should().BeEquivalentTo([2, 4, 6]);
            nonMatches.Should().BeEquivalentTo([1, 3, 5]);
        }

        [TestMethod]
        public void Partition_AllMatch_NonMatchesEmpty()
        {
            (IReadOnlyList<int>? matches, IReadOnlyList<int>? nonMatches) = new[] { 2, 4, 6 }.Partition(x => x % 2 == 0);
            matches.Should().HaveCount(3);
            nonMatches.Should().BeEmpty();
        }

        // --- WhereNotNull ---

        [TestMethod]
        public void WhereNotNull_FiltersNulls()
        {
            string?[] items = ["a", null, "b", null, "c"];
            var result = items.WhereNotNull().ToList();

            result.Should().BeEquivalentTo(["a", "b", "c"]);
        }

        [TestMethod]
        public void WhereNotNull_AllNull_ReturnsEmpty()
        {
            string?[] items = [null, null];
            items.WhereNotNull().Should().BeEmpty();
        }

        [TestMethod]
        public void WhereNotNull_NoNulls_ReturnsAll()
        {
            string?[] items = ["a", "b"];
            items.WhereNotNull().Should().HaveCount(2);
        }
    }
}
