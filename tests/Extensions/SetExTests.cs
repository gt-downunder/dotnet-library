using FluentAssertions;
using Grondo.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class SetExTests : BaseExtensionTest
    {
        [TestMethod]
        public void EqualsSerializedSet_MatchingSets_ReturnsTrue()
        {
            var set = new HashSet<string> { "a", "b", "c" };
            string json = "[\"a\",\"b\",\"c\"]";

            set.EqualsSerializedSet(json).Should().BeTrue();
        }

        [TestMethod]
        public void EqualsSerializedSet_DifferentSets_ReturnsFalse()
        {
            var set = new HashSet<string> { "a", "b" };
            string json = "[\"x\",\"y\"]";

            set.EqualsSerializedSet(json).Should().BeFalse();
        }

        [TestMethod]
        public void EqualsSerializedSet_InvalidJson_ReturnsFalse()
        {
            var set = new HashSet<string> { "a" };
            set.EqualsSerializedSet("not-json").Should().BeFalse();
        }

        [TestMethod]
        public void EqualsSerializedSet_NullSet_ThrowsArgumentNullException()
        {
            ISet<string> set = null!;
            Func<bool> act = () => set.EqualsSerializedSet("[\"a\"]");
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void EqualsSerializedSet_NullOrWhitespaceJson_ThrowsArgumentNullException()
        {
            var set = new HashSet<string> { "a" };
            Func<bool> act = () => set.EqualsSerializedSet("  ");
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void EqualsSerializedSet_IntSets_ReturnsTrue()
        {
            var set = new HashSet<int> { 1, 2, 3 };
            string json = "[1,2,3]";

            set.EqualsSerializedSet(json).Should().BeTrue();
        }
    }
}

