using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using TTMS.UI.Helpers;

namespace TTMS.UI.Tests
{
    [TestFixture]
    public class EnumToListConverterTests
    {
        private enum TestEnum
        {
            [System.ComponentModel.Description("Description1")]
            Item1,
            [System.ComponentModel.Description("Description2")]
            Item2,
            Item3
        }

        private List<EnumToListConverter.ValueDescription> valueDescriptions;
        private Fixture fixture;
        private EnumToListConverter enumToListConverter;

        [SetUp]
        public void SetupTest()
        {
            this.fixture = new Fixture();
            this.enumToListConverter = new EnumToListConverter();

            valueDescriptions = new List<EnumToListConverter.ValueDescription>
            {
                new EnumToListConverter.ValueDescription { Value = TestEnum.Item1, Description = "Description1" },
                new EnumToListConverter.ValueDescription { Value = TestEnum.Item2, Description = "Description2" },
                new EnumToListConverter.ValueDescription { Value = TestEnum.Item3, Description = TestEnum.Item3.ToString() }
            };
        }

        [Test]
        public void Convert_EnumValue_ReturnsListValueDescription()
        {
            // Arrange
            var inputValue = fixture.Create<TestEnum>();

            //Act
            var result = enumToListConverter.Convert(inputValue, It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>());

            //Assert
            Assert.IsInstanceOf(typeof(IEnumerable<EnumToListConverter.ValueDescription>), result, "List of value/description expected");
            var resultList = (result as IEnumerable<EnumToListConverter.ValueDescription>).ToList();
            CollectionAssert.AreEqual(valueDescriptions, resultList, $"Items returned are not the same");
        }

        [Test]
        public void Convert_NonEnumValue_ArgumentException()
        {
            // Arrange
            var inputValue = fixture.Create<string>();

            //Act / Assert
            Assert.Throws<ArgumentException>(() =>
                enumToListConverter.Convert(inputValue, It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>()));

        }
    }
}