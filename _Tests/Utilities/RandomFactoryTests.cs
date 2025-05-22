using DotNet.Library.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net.Mail;

namespace DotNet.Library.Tests.Utilities
{
    [TestClass]
    public class RandomFactoryTests : BaseTest
    {
        [TestMethod]
        public void GetInteger_DefaultParameters_ReturnsInteger()
        {
            // Arrange

            // Act
            var result = RandomFactory.GetInteger();

            // Assert
            result.GetType().Should().Be<int>();
            result.Should().BeGreaterThan(-1);
        }

        [TestMethod]
        public void GetInteger_OneHundredIterations_MinMaxHonored()
        {
            // Arrange
            var generatedValues = new List<int>();

            // Act
            for (int i = 0; i < 100; i++)
            {
                generatedValues.Add(RandomFactory.GetInteger(1, 2));
            }

            // Assert
            generatedValues.Should().AllBeEquivalentTo(1);
        }

        [TestMethod]
        public void GetLong_DefaultParameters_ReturnsLong()
        {
            // Arrange

            // Act
            var result = RandomFactory.GetLong();

            // Assert
            result.GetType().Should().Be<long>();
            result.Should().BeGreaterThan(-1);
        }

        [TestMethod]
        public void GetLong_OneHundredIterations_MinMaxHonored()
        {
            // Arrange
            var generatedValues = new List<long>();

            // Act
            for (int i = 0; i < 100; i++)
            {
                generatedValues.Add(RandomFactory.GetLong(1, 2));
            }

            // Assert
            generatedValues.Should().AllBeEquivalentTo(1);
        }

        [TestMethod]
        public void GetBoolean_OneHundredIterations_TrueAndFalseGenerated()
        {
            // Arrange
            var generatedValues = new List<bool>();

            // Act
            for (int i = 0; i < 100; i++)
            {
                generatedValues.Add(RandomFactory.GetBoolean());
            }

            // Assert
            generatedValues.Should().AllBeAssignableTo<bool>();
        }

        [TestMethod]
        public void GetEmailAddress_OneThousandIterations_ValidAddressesGenerated()
        {
            // Arrange
            var generatedValues = new List<string>();

            // Act
            for (int i = 0; i < 1000; i++)
            {
                generatedValues.Add(RandomFactory.GetEmailAddress(RandomFactory.GetPersonFullName(), RandomFactory.GetCompanyName()));
            }

            // Assert
            generatedValues.Should().AllSatisfy(x =>
            {
                // an invalid email address will throw an exception here
                var _ = new MailAddress(x);
            });
        }
    }
}
