using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Library.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNet.Library.Tests.Extensions
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

            const int size = 10000;
            Dictionary<string, object> hugeDictionary = GetRandomStringObjectDictionary(size);

            var searchArray = new string[10];
            for (var i = 0; i < 10; i++)
            {
                searchArray[i] = hugeDictionary.ElementAt(RandomFactory.GetInteger(0, size)).Key;
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

            var result = dict.HasAnyKey([null]);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void HasAnyKey_ShouldReturnFalse_WhenKeyListIsNull()
        {
            Dictionary<string, object> dict = GetRandomStringObjectDictionary(10);

            var result = dict.HasAnyKey(null);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void TryGetValue_ShouldReturnValue_WhenKeyIsInDictionary()
        {
            var tinyDictionary = new Dictionary<string, object>() { { "one", 1 } };

            tinyDictionary.TryGetValue("one", out var tinyValue).Should().BeTrue();
            tinyValue.Should().Be(1);

            const int size = 10000;
            Dictionary<string, object> hugeDictionary = GetRandomStringObjectDictionary(size);

            for (var i = 0; i < 10; i++)
            {
                var idx = RandomFactory.GetInteger(0, size);
                var key = hugeDictionary.ElementAt(idx).Key;
                var value = hugeDictionary.ElementAt(idx).Value;

                hugeDictionary.TryGetValue(key, out var actual).Should().BeTrue();
                actual.Should().Be(value);
            }
        }

        [TestMethod]
        public void TryGetValue_ShouldReturnDefaultValue_WhenKeyIsNull()
        {
            Dictionary<string, object> dict = GetRandomStringObjectDictionary(10);

            dict.TryGetValue(null!, out var result);

            result.Should().BeNull();
        }

        private static Dictionary<string, object> GetRandomStringObjectDictionary(int size)
        {
            var dict = new Dictionary<string, object>();

            for (var i = 0; dict.Count < size; i++)
            {
                try
                {
                    dict.Add(RandomFactory.GetAlphanumericString(50), i);
                }
                catch (ArgumentException ex)
                {
                    if (ex.Message.Contains("An item with the same key has already been added."))
                    {
                        // swallow, it's reasonable, as unlikely as it may be, that we might generate the same random value twice
                    }
                }
            }

            return dict;
        }
    }
}