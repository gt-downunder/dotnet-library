using DotNet.Library.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DotNet.Library.Tests.Extensions
{
    [TestClass]
    public class StringExTests : BaseExtensionTest
    {
        [TestMethod]
        public void IsWellFormedEmail_ShouldReturnTrue_WhenEmailsAreCorrect()
        {
            string[] goodEmails = [ "david.jones@contoso.com", "d.j@server1.contoso.com",
                                    "jones@ms1.contoso.com", "j@contoso.com9",
                                    "js#internal@contoso.com", "j_9@[129.126.118.1]",
                                    "js@contoso.com9", "j.s@server1.contoso.com",
                                    "\"j\\\"s\\\"\"@contoso.com", "js@contoso.中国" ];

            foreach (string emailAddress in goodEmails)
            {
                emailAddress.IsWellFormedEmailAddress().Should().BeTrue();
            }
        }

        [TestMethod]
        public void IsWellFormedEmail_ShouldReturnFalse_WhenEmailsAreIncorrect()
        {
            string[] badEmails = [ "j.@@.contoso.com", "j@..s@contoso.com",
                                   "js*@contoso.com.@", "js@contoso.@.com" ];

            foreach (string emailAddress in badEmails)
            {
                emailAddress.IsWellFormedEmailAddress().Should().BeFalse();
            }
        }

        [TestMethod]
        public void EqualsIgnoreCaseWithTrim_ShouldReturnTrue_WhenStringEndsWithEmptySpace()
        {
            "Test Value".EqualsIgnoreCaseWithTrim("test value ").Should().BeTrue();
        }

        [TestMethod]
        public void EqualsIgnoreCaseWithTrim_ShouldReturnFalse_WhenStringsAreNotEqual()
        {
            "Test Value".EqualsIgnoreCaseWithTrim("test value&%$").Should().BeFalse();
        }

        [TestMethod]
        public void EqualsWithTrim_ShouldReturnTrue_WhenStringEndsWithEmptySpace()
        {
            "Test Value".EqualsWithTrim("Test Value ").Should().BeTrue();
        }

        [TestMethod]
        public void EqualsWithTrim_ShouldReturnFalse_WhenStringAreNotEqual()
        {
            "Test Value".EqualsWithTrim("Test Value&%$").Should().BeFalse();
        }

        [TestMethod]
        public void StartsWithIgnoreCaseWithTrim_ShouldReturnTrue_WhenStringStartWithEmptySpace()
        {
            "Test Value".StartsWithIgnoreCaseWithTrim(" Test").Should().BeTrue();
        }

        [TestMethod]
        public void StartsWithIgnoreCaseWithTrim_ShouldReturnFalse_WhenStringDoNotStartWithValue()
        {
            "Test Value".StartsWithIgnoreCaseWithTrim(" Test_").Should().BeFalse();
        }

        [TestMethod]
        public void Split_SeparatorNotInString_NotSplit()
        {
            const string input = "This is a regular old sentence, nothing special to see here.";
            string[] result = input.Split("fox");

            result.Length.Should().Be(1);
            result.Single().Should().Be(input);
        }

        [TestMethod]
        public void SplitIgnoreCase_SeparatorCaseDoesntMatch_CaseInsensitiveSplit()
        {
            const string input = "HELLO WORLD! WE ARE SO EXCITED";
            string[] result = input.SplitIgnoreCase("world");

            result.Length.Should().Be(2);
            result[0].Should().Be("HELLO ");
            result[^1].Should().Be("! WE ARE SO EXCITED");
        }

        [TestMethod]
        public void SplitIgnoreCase_SeparatorCaseMatches_CaseInsensitiveSplit()
        {
            const string input = "HELLO WORLD! WE ARE SO EXCITED";
            string[] result = input.SplitIgnoreCase("WORLD");

            result.Length.Should().Be(2);
            result[0].Should().Be("HELLO ");
            result[^1].Should().Be("! WE ARE SO EXCITED");
        }

        [TestMethod]
        public void Split_IgnoreCaseFalseOrNotPassed_NotSplit()
        {
            const string input = "HELLO WORLD! WE ARE SO EXCITED";
            string[] falseResult = input.Split("world", ignoreCase: false);
            string[] nullResult = input.Split("world");

            falseResult.Length.Should().Be(1);
            falseResult.Single().Should().Be(input);

            nullResult.Length.Should().Be(1);
            nullResult.Single().Should().Be(input);
        }

        [TestMethod]
        public void Split_IgnoreCaseTrue_CaseInsensitiveSplit()
        {
            const string input = "HELLO WORLD! WE ARE SO EXCITED";
            string[] result = input.Split("world", ignoreCase: true);

            result.Length.Should().Be(2);
            result[0].Should().Be("HELLO ");
            result[^1].Should().Be("! WE ARE SO EXCITED");
        }
    }
}
