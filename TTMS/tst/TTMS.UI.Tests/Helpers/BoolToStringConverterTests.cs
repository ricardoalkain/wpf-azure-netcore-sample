using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TTMS.UI.Helpers;
using AutoFixture;
using Moq;
using System.Globalization;

namespace TTMS.UI.Tests
{
    [TestClass]
    public class BoolToStringConverterTests
    {
        private Fixture fixture;
        private BoolToStringConverter boolToStringConverter;

        [TestInitialize]
        public void SetupTest()
        {
            this.fixture = new Fixture();
            this.boolToStringConverter = new BoolToStringConverter();
        }

        [TestMethod]
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
            Assert.IsInstanceOfType(result, typeof(string), "String result expected");
            Assert.AreEqual(expected, result.ToString(), false, $"Test returned '{result}' but {expected} was expected");
        }

        [TestMethod]
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
            Assert.IsInstanceOfType(result, typeof(string), "String result expected");
            Assert.AreEqual(expected, result.ToString(), false, $"Test returned '{result}' but {expected} was expected");
        }

        [TestMethod]
        public void Convert_TrueValueNullString_ReturnsTrueString()
        {
            // Arrange
            var inputValue = true;
            string stringParam = null;

            //Act
            var result = boolToStringConverter.Convert(inputValue, It.IsAny<Type>(), stringParam, It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOfType(result, typeof(string), "String result expected");
            Assert.AreEqual(bool.TrueString, result.ToString(), false, $"Test returned '{result}' but {bool.TrueString} was expected");
        }

        [TestMethod]
        public void Convert_FalseValueNullString_ReturnsFalseString()
        {
            // Arrange
            var inputValue = false;
            string stringParam = null;

            //Act
            var result = boolToStringConverter.Convert(inputValue, It.IsAny<Type>(), stringParam, It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOfType(result, typeof(string), "String result expected");
            Assert.AreEqual(bool.FalseString, result.ToString(), false, $"Test returned '{result}' but {bool.FalseString} was expected");
        }
    }
}
