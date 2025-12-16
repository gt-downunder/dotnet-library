using System.Collections.Generic;
using System.Linq;
using DotNet.Library.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNet.Library.Tests.Extensions
{
    [TestClass]
    public class GenericListExTests : BaseExtensionTest
    {
        [TestMethod]
        public void Add_EmptyStringListWithValidItems_Success()
        {
            // Arrange
            var itemsToAdd = new string[] { "1", "2", "3" };
            var list = new List<string>
            {
                // Act
                itemsToAdd
            };

            // Assert
            list.Count.Should().Be(itemsToAdd.Length);
            itemsToAdd.ForEach((item, _) => list.Should().Contain(item));
        }

        [TestMethod]
        public void Add_NonEmptyStringListWithValidItems_Success()
        {
            // Arrange
            var itemsToAdd = new string[] { "1", "2", "3" };
            var existingItems = new string[] { "Hello", "World", "How are", "you today?" };
            var list = new List<string>
            {
                existingItems,
                // Act
                itemsToAdd
            };

            // Assert
            list.Count.Should().Be(itemsToAdd.Length + existingItems.Length);
            itemsToAdd.ForEach((item, _) => list.Should().Contain(item));
            existingItems.ForEach((item, _) => list.Should().Contain(item));
        }

        [TestMethod]
        public void Add_EmptyIntListWithValidItems_Success()
        {
            // Arrange
            var itemsToAdd = new int[] { 1, 2, 3 };
            var list = new List<int>
            {
                // Act
                itemsToAdd
            };

            // Assert
            list.Count.Should().Be(itemsToAdd.Length);
            itemsToAdd.ForEach((item, _) => list.Should().Contain(item));
        }

        [TestMethod]
        public void Add_NonEmptyIntListWithValidItems_Success()
        {
            // Arrange
            var itemsToAdd = new int[] { 1, 2, 3 };
            var existingItems = new int[] { 100, 200, 300, 400 };
            var list = new List<int>
            {
                existingItems,
                // Act
                itemsToAdd
            };

            // Assert
            list.Count.Should().Be(itemsToAdd.Length + existingItems.Length);
            itemsToAdd.ForEach((item, _) => list.Should().Contain(item));
            existingItems.ForEach((item, _) => list.Should().Contain(item));
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void AddIfNotNull_Boolean_Added(bool testValue)
        {
            // Arrange
            var list = new List<bool>();

            // Act
            list.AddIfNotNull(testValue);

            // Assert
            list.Should().Contain(testValue);
        }

        [TestMethod]
        [DataRow("Hello World")]
        [DataRow("The quick brown fox jumps over the lazy dog")]
        public void AddIfNotNull_String_Added(string testValue)
        {
            // Arrange
            var list = new List<string>();

            // Act
            list.AddIfNotNull(testValue);

            // Assert
            list.Should().Contain(testValue);
        }

        [TestMethod]
        [DataRow(42)]
        [DataRow(123)]
        public void AddIfNotNull_Int_Added(int testValue)
        {
            // Arrange
            var list = new List<int>();

            // Act
            list.AddIfNotNull(testValue);

            // Assert
            list.Should().Contain(testValue);
        }

        [TestMethod]
        public void AddIfNotNull_NullString_NotAdded()
        {
            // Arrange
            var list = new List<string>();
            const string testValue = null;

            // Act
            list.AddIfNotNull(testValue);

            // Assert
            list.Should().BeEmpty();
        }

        [TestMethod]
        [DataRow("hello  world")]
        [DataRow("The quick brown fox jumps over the lazy dog")]
        [DataRow("")]
        public void AddIfNotExists_ListDoesNotContainNewValue_NewValueAdded(string existingValue)
        {
            // Arrange
            var list = new List<string> { existingValue };
            const string newValue = "hello world";

            // Act
            list.AddIfNotExists(newValue);

            // Assert
            list.Count.Should().Be(2);
            list.Should().Contain(existingValue);
            list.Should().Contain(newValue);
        }

        [TestMethod]
        [DataRow("Hello world")]
        [DataRow("hello world")]
        [DataRow("HeLlO WoRlD")]
        public void AddIfNotExists_ListContainsValue_ListUnchanged(string testValue)
        {
            // Arrange
            var list = new List<string> { "HELLO WORLD" };

            // Act
            list.AddIfNotExists(testValue);

            // Assert
            list.Count.Should().Be(1);
            list.Should().Contain(list.Single());
        }

        [TestMethod]
        public void AddRangeNoDuplicates_AddSomeDuplicates_ResultIsDistinct()
        {
            // Arrange
            var newValues = new List<string>
            {
                "Hello",
                "How are",
                "you doing"
            };

            var existingValues = new string[] { "hello", "world", "how are", "you today" };
            var list = new List<string> { existingValues };

            // Act
            list.AddRangeNoDuplicates(newValues);

            // Assert
            list.Count.Should().Be(5);
            existingValues.ForEach((value, _) => list.Should().Contain(value));
            list.Should().Contain("you doing");
        }

        [TestMethod]
        public void AddRangeNoDuplicates_AddNoDuplicates_AllValuesAdded()
        {
            // Arrange
            var newValues = new List<string>
            {
                "Hello,",
                "How are ",
                "you doing?"
            };

            var existingValues = new string[] { "hello", "world", "how are", "you today" };
            var list = new List<string> { existingValues };

            // Act
            list.AddRangeNoDuplicates(newValues);

            // Assert
            list.Count.Should().Be(7);
            existingValues.ForEach((value, _) => list.Should().Contain(value));
            newValues.ForEach((value, _) => list.Should().Contain(value));
        }

        [TestMethod]
        public void ContainsIgnoreCase_DoesContainMatchingCase_ReturnsTrue()
        {
            // Arrange
            const string testValue = "Hello world";
            var list = new List<string> { { testValue } };

            // Act
            var result = list.ContainsIgnoreCase(testValue);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow("HELLO WORLD")]
        [DataRow("hello world")]
        [DataRow("HeLlO WoRlD")]
        public void ContainsIgnoreCase_DoesContainDifferentCase_ReturnsTrue(string testValue)
        {
            // Arrange
            var list = new List<string> { { "Hello world" } };

            // Act
            var result = list.ContainsIgnoreCase(testValue.ToLower());

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(" HELLO WORLD")]
        [DataRow("hello  world")]
        [DataRow("HeLlO WoRlD ")]
        public void ContainsIgnoreCase_WhitespaceDifferenceOnly_ReturnsFalse(string testValue)
        {
            // Arrange
            var list = new List<string> { { "Hello world" } };

            // Act
            var result = list.ContainsIgnoreCase(testValue.ToLower());

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow("HELLO WORLD")]
        [DataRow("hello world")]
        [DataRow("HeLlO WoRlD")]
        public void ContainsIgnoreCase_ManyValuesInListWithDifferentCaseMatch_ReturnsTrue(string testValue)
        {
            // Arrange
            var list = new List<string> { { "Hello world" } };

            for (int i = 0; i < 100; i++)
            {
                list.Add(RandomFactory.GetAlphanumericString(100));
            }

            // Act
            var result = list.ContainsIgnoreCase(testValue.ToLower());

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow("Hello world")]
        public void ContainsIgnoreCase_ManyValuesInListWithExactCaseMatch_ReturnsTrue(string testValue)
        {
            // Arrange
            var list = new List<string> { { "Hello world" } };

            for (int i = 0; i < 100; i++)
            {
                list.Add(RandomFactory.GetAlphanumericString(100));
            }

            // Act
            var result = list.ContainsIgnoreCase(testValue.ToLower());

            // Assert
            result.Should().BeTrue();
        }
    }
}