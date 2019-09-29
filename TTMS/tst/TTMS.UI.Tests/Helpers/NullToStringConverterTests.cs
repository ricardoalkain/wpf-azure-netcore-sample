using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TTMS.UI.Helpers;
using AutoFixture;
using Moq;
using System.Globalization;

namespace TTMS.UI.Tests
{
    [TestClass]
    public class NullToStringConverterTests
    {
        private Fixture fixture;
        private NullToStringConverter nullToStringConverter;

        [TestInitialize]
        public void SetupTest()
        {
            this.fixture = new Fixture();
            this.nullToStringConverter = new NullToStringConverter();
        }

        [TestMethod]
        public void Convert_NullValue_ReturnsFirstString()
        {
            // Arrange
            string inputValue = null;
            var expected = fixture.Create<string>();
            var value2 = fixture.Create<string>();
            var stringParam = $"{expected},{value2}";

            //Act
            var result = nullToStringConverter.Convert(inputValue, It.IsAny<Type>(), stringParam, It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOfType(result, typeof(string), "String result expected");
            Assert.AreEqual(expected, result.ToString(), false, $"Test returned '{result}' but {expected} was expected");
        }

        [TestMethod]
        public void Convert_NonNullValue_ReturnsSecondString()
        {
            // Arrange
            var inputValue = fixture.Create<string>();
            var value1 = fixture.Create<string>();
            var expected = fixture.Create<string>();
            var stringParam = $"{value1},{expected}";

            //Act
            var result = nullToStringConverter.Convert(inputValue, It.IsAny<Type>(), stringParam, It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOfType(result, typeof(string), "String result expected");
            Assert.AreEqual(expected, result.ToString(), false, $"Test returned '{result}' but {expected} was expected");
        }

        [TestMethod]
        public void Convert_NullValueNullString_ReturnsTrueString()
        {
            // Arrange
            string inputValue = null;
            string stringParam = null;

            //Act
            var result = nullToStringConverter.Convert(inputValue, It.IsAny<Type>(), stringParam, It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOfType(result, typeof(string), "String result expected");
            Assert.AreEqual(bool.TrueString, result.ToString(), false, $"Test returned '{result}' but {bool.TrueString} was expected");
        }

        [TestMethod]
        public void Convert_FalseValueNullString_ReturnsFalseString()
        {
            // Arrange
            var inputValue = fixture.Create<string>();
            string stringParam = null;

            //Act
            var result = nullToStringConverter.Convert(inputValue, It.IsAny<Type>(), stringParam, It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOfType(result, typeof(string), "String result expected");
            Assert.AreEqual(bool.FalseString, result.ToString(), false, $"Test returned '{result}' but {bool.FalseString} was expected");
        }
    }
}