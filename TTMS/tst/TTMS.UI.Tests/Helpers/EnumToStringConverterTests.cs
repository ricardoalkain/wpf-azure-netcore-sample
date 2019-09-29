using System;
using System.Globalization;
using AutoFixture;
using Moq;
using NUnit.Framework;
using TTMS.UI.Helpers;

namespace TTMS.UI.Tests
{
    [TestFixture]
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

        [SetUp]
        public void SetupTest()
        {
            this.fixture = new Fixture();
            this.enumToStringConverter = new EnumToStringConverter();
        }

        [Test]
        public void Convert_EnumValueWithDescription_ReturnsDescription()
        {
            // Arrange
            var inputValue = TestEnum.Item1;
            var expected = "Description1";

            //Act
            var result = enumToStringConverter.Convert(inputValue, It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOf(typeof(string), result, "String result expected");
            Assert.AreEqual(expected, result.ToString(), $"Test returned '{result}' but '{expected}' was expected");
        }

        [Test]
        public void Convert_EnumValueWithoutDescription_ReturnsItemName()
        {
            // Arrange
            var inputValue = TestEnum.Item3;
            var expected = inputValue.ToString();

            //Act
            var result = enumToStringConverter.Convert(inputValue, It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOf(typeof(string), result, "String result expected");
            Assert.AreEqual(expected, result.ToString(), $"Test returned '{result}' but '{expected}' was expected");
        }

        [Test]
        public void Convert_NonEnumValue_ArgumentException()
        {
            // Arrange
            var inputValue = fixture.Create<string>();

            //Act / Assert
            Assert.Throws<ArgumentException>(() =>
                enumToStringConverter.Convert(inputValue, It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>()));
        }
    }
}