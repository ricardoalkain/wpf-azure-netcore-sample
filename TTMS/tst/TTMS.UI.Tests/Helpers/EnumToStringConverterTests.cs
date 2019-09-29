using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TTMS.UI.Helpers;
using AutoFixture;
using Moq;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace TTMS.UI.Tests
{
    [TestClass]
    public class EnumToStringConverterTests
    {
        private enum TestEnum
        {
            [System.ComponentModel.Description("Description1")]
            Item1,
            [System.ComponentModel.Description("Description2")]
            Item2,
            Item3
        }

        private Fixture fixture;
        private EnumToStringConverter enumToStringConverter;

        [TestInitialize]
        public void SetupTest()
        {
            this.fixture = new Fixture();
            this.enumToStringConverter = new EnumToStringConverter();
        }

        [TestMethod]
        public void Convert_EnumValueWithDescription_ReturnsDescription()
        {
            // Arrange
            var inputValue = TestEnum.Item1;
            var expected = "Description1";

            //Act
            var result = enumToStringConverter.Convert(inputValue, It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOfType(result, typeof(string), "String result expected");
            Assert.AreEqual(expected, result.ToString(), false, $"Test returned '{result}' but '{expected}' was expected");
        }

        [TestMethod]
        public void Convert_EnumValueWithoutDescription_ReturnsItemName()
        {
            // Arrange
            var inputValue = TestEnum.Item3;
            var expected = inputValue.ToString();

            //Act
            var result = enumToStringConverter.Convert(inputValue, It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOfType(result, typeof(string), "String result expected");
            Assert.AreEqual(expected, result.ToString(), false, $"Test returned '{result}' but '{expected}' was expected");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Convert_NonEnumValue_ArgumentException()
        {
            // Arrange
            var inputValue = fixture.Create<string>();

            //Act
            var result = enumToStringConverter.Convert(inputValue, It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>());

            //Assert - Exception
        }
    }
}