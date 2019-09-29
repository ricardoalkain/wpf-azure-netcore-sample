using System;
using TTMS.UI.Helpers;
using AutoFixture;
using Moq;
using System.Globalization;
using NUnit.Framework;
using FluentAssertions;

namespace TTMS.UI.Tests
{
    [TestFixture]
    public class BoolToStringConverterTests
    {
        private Fixture fixture;
        private BoolToStringConverter boolToStringConverter;

        [SetUp]
        public void SetupTest()
        {
            this.fixture = new Fixture();
            this.boolToStringConverter = new BoolToStringConverter();
        }

        [Test]
        public void Convert_TrueValue_ReturnsFirstString()
        {
            // Arrange
            var inputValue = true;
            var expected = fixture.Create<string>();
            var value2 = fixture.Create<string>();
            var stringParam = $"{expected},{value2}";

            //Act
            var result = boolToStringConverter.Convert(inputValue, It.IsAny<Type>(), stringParam, It.IsAny<CultureInfo>());

            //Assert
            result.Should().BeOfType(typeof(string), "String result expected");
            expected.Should().Be(result.ToString(), $"Test returned '{result}' but {expected} was expected");
        }

        [Test]
        public void Convert_FalseValue_ReturnsSecondString()
        {
            // Arrange
            var inputValue = false;
            var value1 = fixture.Create<string>();
            var expected = fixture.Create<string>();
            var stringParam = $"{value1},{expected}";

            //Act
            var result = boolToStringConverter.Convert(inputValue, It.IsAny<Type>(), stringParam, It.IsAny<CultureInfo>());

            //Assert
            result.Should().BeOfType(typeof(string), "String result expected");
            expected.Should().Be(result.ToString(), $"Test returned '{result}' but {expected} was expected");
        }

        [Test]
        public void Convert_TrueValueNullString_ReturnsTrueString()
        {
            // Arrange
            var inputValue = true;
            string stringParam = null;

            //Act
            var result = boolToStringConverter.Convert(inputValue, It.IsAny<Type>(), stringParam, It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOf(typeof(string), result, "String result expected");
            Assert.AreEqual(bool.TrueString, result.ToString(), $"Test returned '{result}' but {bool.TrueString} was expected");
        }

        [Test]
        public void Convert_FalseValueNullString_ReturnsFalseString()
        {
            // Arrange
            var inputValue = false;
            string stringParam = null;

            //Act
            var result = boolToStringConverter.Convert(inputValue, It.IsAny<Type>(), stringParam, It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOf(typeof(string), result, "String result expected");
            Assert.AreEqual(bool.FalseString, result.ToString(), $"Test returned '{result}' but {bool.FalseString} was expected");
        }
    }
}
