using FluentAssertions;
using Grondo.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class DictionaryExTests : BaseExtensionTest
    {
        [TestMethod]
        public void HasAnyKey_ShouldReturnTrue_WhenKeyIsInDictionary()
        {
            var tinyDictionary = new Dictionary<string, object>() { { "one", 1 } };

            tinyDictionary.HasAnyKey(["one"]).Should().BeTrue();
            tinyDictionary.HasAnyKey(["one", "two"]).Should().BeTrue();

            const int Size = 10000;
            Dictionary<string, object> hugeDictionary = GetRandomStringObjectDictionary(Size);

            string[] searchArray = new string[10];
            for (int i = 0; i < 10; i++)
            {
                searchArray[i] = hugeDictionary.ElementAt(RandomFactory.GetInteger(0, Size)).Key;
            }

            hugeDictionary.HasAnyKey(searchArray).Should().BeTrue();
            hugeDictionary.HasAnyKey([Guid.NewGuid().ToString()]).Should().BeFalse();
        }

        [TestMethod]
        public void HasAnyKey_ShouldReturnFalse_WhenKeyIsNotInDictionary()
        {
            var dict = new Dictionary<string, object> { { "one", 1 } };

            dict.HasAnyKey(["two", "three", "four"]).Should().BeFalse();
        }

        [TestMethod]
        public void HasAnyKey_ShouldReturnFalse_WhenOneKeyIsNull()
        {
            Dictionary<string, object> dict = GetRandomStringObjectDictionary(10);

            bool result = dict.HasAnyKey([null!]);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void HasAnyKey_ShouldReturnFalse_WhenKeyListIsNull()
        {
            Dictionary<string, object> dict = GetRandomStringObjectDictionary(10);

            bool result = dict.HasAnyKey(null);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void TryGetValue_ShouldReturnValue_WhenKeyIsInDictionary()
        {
            var tinyDictionary = new Dictionary<string, object>() { { "one", 1 } };

            tinyDictionary.TryGetValue("one", out object? tinyValue).Should().BeTrue();
            tinyValue.Should().Be(1);

            const int Size = 10000;
            Dictionary<string, object> hugeDictionary = GetRandomStringObjectDictionary(Size);

            for (int i = 0; i < 10; i++)
            {
                int idx = RandomFactory.GetInteger(0, Size);
                string key = hugeDictionary.ElementAt(idx).Key;
                object value = hugeDictionary.ElementAt(idx).Value;

                hugeDictionary.TryGetValue(key, out object? actual).Should().BeTrue();
                actual.Should().Be(value);
            }
        }

        [TestMethod]
        public void TryGetValue_ShouldThrowArgumentNullException_WhenKeyIsNull()
        {
            Dictionary<string, object> dict = GetRandomStringObjectDictionary(10);

            Action act = () => dict.TryGetValue(null!, out _);
            act.Should().Throw<ArgumentNullException>();
        }

        private static Dictionary<string, object> GetRandomStringObjectDictionary(int size)
        {
            var dict = new Dictionary<string, object>();

            for (int i = 0; dict.Count < size; i++)
            {
                dict.TryAdd(RandomFactory.GetAlphanumericString(50), i);
            }

            return dict;
        }

        // --- Merge ---

        [TestMethod]
        public void Merge_Overwrite_MergesAndOverwrites()
        {
            var dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
            var other = new Dictionary<string, int> { { "b", 99 }, { "c", 3 } };

            dict.Merge(other);

            dict["a"].Should().Be(1);
            dict["b"].Should().Be(99);
            dict["c"].Should().Be(3);
        }

        [TestMethod]
        public void Merge_NoOverwrite_PreservesExisting()
        {
            var dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
            var other = new Dictionary<string, int> { { "b", 99 }, { "c", 3 } };

            dict.Merge(other, overwrite: false);

            dict["b"].Should().Be(2); // preserved
            dict["c"].Should().Be(3); // added
        }

        [TestMethod]
        public void Merge_EmptyOther_NoChange()
        {
            var dict = new Dictionary<string, int> { { "a", 1 } };
            dict.Merge(new Dictionary<string, int>());
            dict.Should().HaveCount(1);
        }

        // --- GetOrAdd ---

        [TestMethod]
        public void GetOrAdd_KeyExists_ReturnsExisting()
        {
            var dict = new Dictionary<string, int> { { "a", 42 } };
            int result = dict.GetOrAdd("a", () => 999);
            result.Should().Be(42);
        }

        [TestMethod]
        public void GetOrAdd_KeyNotExists_CreatesAndAdds()
        {
            var dict = new Dictionary<string, int>();
            int result = dict.GetOrAdd("a", () => 42);

            result.Should().Be(42);
            dict["a"].Should().Be(42);
        }

        [TestMethod]
        public void GetOrAdd_FactoryCalledOnlyOnce()
        {
            var dict = new Dictionary<string, int>();
            int callCount = 0;

            dict.GetOrAdd("a", () => { callCount++; return 42; });
            dict.GetOrAdd("a", () => { callCount++; return 99; });

            callCount.Should().Be(1);
        }
    }
}
