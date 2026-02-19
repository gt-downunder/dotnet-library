using FluentAssertions;
using Grondo.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class StringExTests : BaseExtensionTest
    {
        [TestMethod]
        public void IsWellFormedEmail_ShouldReturnTrue_WhenEmailsAreCorrect()
        {
            string[] goodEmails =
            [
                "david.jones@contoso.com", "d.j@server1.contoso.com",
                "jones@ms1.contoso.com", "j@contoso.com9",
                "js#internal@contoso.com", "j_9@[129.126.118.1]",
                "js@contoso.com9", "j.s@server1.contoso.com",
                "\"j\\\"s\\\"\"@contoso.com", "js@contoso.中国"
            ];

            foreach (string emailAddress in goodEmails)
            {
                emailAddress.IsWellFormedEmailAddress().Should().BeTrue();
            }
        }

        [TestMethod]
        public void IsWellFormedEmail_ShouldReturnFalse_WhenEmailsAreIncorrect()
        {
            string[] badEmails =
            [
                "j.@@.contoso.com", "j@..s@contoso.com",
                "js*@contoso.com.@", "js@contoso.@.com"
            ];

            foreach (string emailAddress in badEmails)
            {
                emailAddress.IsWellFormedEmailAddress().Should().BeFalse();
            }
        }

        [TestMethod]
        public void EqualsIgnoreCaseWithTrim_ShouldReturnTrue_WhenStringEndsWithEmptySpace() => "Test Value".EqualsIgnoreCaseWithTrim("test value ").Should().BeTrue();

        [TestMethod]
        public void EqualsIgnoreCaseWithTrim_ShouldReturnFalse_WhenStringsAreNotEqual() => "Test Value".EqualsIgnoreCaseWithTrim("test value&%$").Should().BeFalse();

        [TestMethod]
        public void EqualsWithTrim_ShouldReturnTrue_WhenStringEndsWithEmptySpace() => "Test Value".EqualsWithTrim("Test Value ").Should().BeTrue();

        [TestMethod]
        public void EqualsWithTrim_ShouldReturnFalse_WhenStringAreNotEqual() => "Test Value".EqualsWithTrim("Test Value&%$").Should().BeFalse();

        [TestMethod]
        public void StartsWithIgnoreCaseWithTrim_ShouldReturnTrue_WhenStringStartWithEmptySpace() => "Test Value".StartsWithIgnoreCaseWithTrim(" Test").Should().BeTrue();

        [TestMethod]
        public void StartsWithIgnoreCaseWithTrim_ShouldReturnFalse_WhenStringDoNotStartWithValue() => "Test Value".StartsWithIgnoreCaseWithTrim(" Test_").Should().BeFalse();

        [TestMethod]
        public void RegexSplit_SeparatorNotInString_NotSplit()
        {
            const string Input = "This is a regular old sentence, nothing special to see here.";
            string[] result = Input.RegexSplit("fox");

            result.Length.Should().Be(1);
            result.Single().Should().Be(Input);
        }

        [TestMethod]
        public void RegexSplit_IgnoreCaseFalseOrNotPassed_NotSplit()
        {
            const string Input = "HELLO WORLD! WE ARE SO EXCITED";
            string[] falseResult = Input.RegexSplit("world", ignoreCase: false);
            string[] nullResult = Input.RegexSplit("world");

            falseResult.Length.Should().Be(1);
            falseResult.Single().Should().Be(Input);

            nullResult.Length.Should().Be(1);
            nullResult.Single().Should().Be(Input);
        }

        [TestMethod]
        public void RegexSplit_IgnoreCaseTrue_CaseInsensitiveSplit()
        {
            const string Input = "HELLO WORLD! WE ARE SO EXCITED";
            string[] result = Input.RegexSplit("world", ignoreCase: true);

            result.Length.Should().Be(2);
            result[0].Should().Be("HELLO ");
            result[^1].Should().Be("! WE ARE SO EXCITED");
        }

        // --- Truncate ---

        [TestMethod]
        public void Truncate_ShorterThanMax_ReturnsOriginal() => "hello".Truncate(10).Should().Be("hello");

        [TestMethod]
        public void Truncate_LongerThanMax_TruncatesWithSuffix() => "hello world".Truncate(8).Should().Be("hello...");

        [TestMethod]
        public void Truncate_ExactLength_ReturnsOriginal() => "hello".Truncate(5).Should().Be("hello");

        [TestMethod]
        public void Truncate_MaxLengthZero_ReturnsEmpty() => "hello".Truncate(0).Should().BeEmpty();

        [TestMethod]
        public void Truncate_NegativeMaxLength_Throws()
        {
            Action act = () => "hello".Truncate(-1);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Truncate_CustomSuffix_Uses() => "hello world".Truncate(7, "~~").Should().Be("hello~~");

        // --- ToBase64 / FromBase64 ---

        [TestMethod]
        public void ToBase64_RoundTrips()
        {
            const string Original = "Hello, World!";
            string encoded = Original.ToBase64();
            encoded.Should().NotBe(Original);
            encoded.FromBase64().Should().Be(Original);
        }

        [TestMethod]
        public void ToBase64_EmptyString_ReturnsEmpty() => "".ToBase64().Should().BeEmpty();

        // --- Mask ---

        [TestMethod]
        public void Mask_DefaultVisibleChars_ShowsLast4() => "4111111111111111".Mask().Should().Be("************1111");

        [TestMethod]
        public void Mask_CustomVisibleChars() => "secret123".Mask(3).Should().Be("******123");

        [TestMethod]
        public void Mask_ShortString_ReturnsOriginal() => "abc".Mask(4).Should().Be("abc");

        [TestMethod]
        public void Mask_CustomMaskChar() => "secret".Mask(2, '#').Should().Be("####et");

        // --- ToSlug ---

        [TestMethod]
        public void ToSlug_BasicString_ReturnsSlug() => "Hello World!".ToSlug().Should().Be("hello-world");

        [TestMethod]
        public void ToSlug_SpecialChars_Removed() => "C# is great!!!".ToSlug().Should().Be("c-is-great");

        [TestMethod]
        public void ToSlug_AlreadySlug_ReturnsSame() => "already-a-slug".ToSlug().Should().Be("already-a-slug");

        // --- IsNullOrWhiteSpace ---

        [TestMethod]
        public void IsNullOrWhiteSpace_Null_ReturnsTrue() => ((string?)null).IsNullOrWhiteSpace().Should().BeTrue();

        [TestMethod]
        public void IsNullOrWhiteSpace_Empty_ReturnsTrue() => "".IsNullOrWhiteSpace().Should().BeTrue();

        [TestMethod]
        public void IsNullOrWhiteSpace_Whitespace_ReturnsTrue() => "   ".IsNullOrWhiteSpace().Should().BeTrue();

        [TestMethod]
        public void IsNullOrWhiteSpace_HasContent_ReturnsFalse() => "hello".IsNullOrWhiteSpace().Should().BeFalse();
    }
}
