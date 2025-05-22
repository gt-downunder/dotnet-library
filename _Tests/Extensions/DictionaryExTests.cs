using DotNet.Library.Extensions;
using DotNet.Library.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Library.Tests.Extensions
{
    [TestClass]
    public class DictionaryExTests : BaseExtensionTest
    {
        [TestMethod]
        public void HasAnyKey_ShouldReturnTrue_WhenKeyIsInDicitonary()
        {
            var tinyDictionary = new Dictionary<string, object>() { { "one", 1 } };

            tinyDictionary.HasAnyKey(["one"]).Should().BeTrue();
            tinyDictionary.HasAnyKey(["one", "two"]).Should().BeTrue();

            const int size = 10000;
            var hugeDictionary = GetRandomStringObjectDictionary(size);

            var searchArray = new string[10];
            for (int i = 0; i < 10; i++)
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
            var dict = GetRandomStringObjectDictionary(10);

            var result = dict.HasAnyKey([null]);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void HasAnyKey_ShouldReturnFalse_WhenKeyListIsNull()
        {
            var dict = GetRandomStringObjectDictionary(10);

            var result = dict.HasAnyKey(null);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void TryGetValue_ShouldReturnValue_WhenKeyIsInDicitonary()
        {
            var tinyDictionary = new Dictionary<string, object>() { { "one", 1 } };

            tinyDictionary.TryGetValue("one").Should().NotBeNull();
            tinyDictionary.TryGetValue("one").Should().Be(1);

            const int size = 10000;
            var hugeDictionary = GetRandomStringObjectDictionary(size);

            for (int i = 0; i < 10; i++)
            {
                int idx = RandomFactory.GetInteger(0, size);
                var key = hugeDictionary.ElementAt(idx).Key;
                var value = hugeDictionary.ElementAt(idx).Value;

                hugeDictionary.TryGetValue(key).Should().Be(value);
            }
        }

        [TestMethod]
        public void TryGetValue_ShouldReturnDefaultValue_WhenKeyIsNull()
        {
            var dict = GetRandomStringObjectDictionary(10);

            var result = dict.TryGetValue(null);

            result.Should().BeNull();
        }

        private static Dictionary<string, object> GetRandomStringObjectDictionary(int size)
        {
            var dict = new Dictionary<string, object>();

            for (int i = 0; dict.Count < size; i++)
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
