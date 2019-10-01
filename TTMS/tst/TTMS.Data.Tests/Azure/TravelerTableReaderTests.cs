using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using NUnit.Framework;
using TTMS.Common.Entities.Extensions;
using TTMS.Common.Enums;
using TTMS.Data.Abstractions;
using TTMS.Data.Azure;
using TTMS.Data.Tests.Mocks;
using Entities = TTMS.Common.Entities;
using Models = TTMS.Common.Models;

namespace TTMS.Data.Tests.Azure
{
    [TestFixture]
    public class TravelerTableReaderTests
    {
        private ILogger<TravelerTableReader> logger;
        private IConfiguration config;
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger<TravelerTableReader>>().Object;
            config = new Mock<IConfiguration>().Object;
            fixture = new Fixture();
        }

        [Test]
        public async Task GetAllAsync_HasData_TravelerList()
        {
            // Arrange
            var entities = CreateEntities();
            var tableReader = MockAzureCloudObjects(entities);
            var expected = entities.ToModel();

            // Act
            var result = await tableReader.GetAllAsync();

            // Assert
            Assert.IsInstanceOf<IEnumerable<Models.Traveler>>(result);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAllAsync_NoData_EmptyList()
        {
            // Arrange
            var entities = Enumerable.Empty<Entities.Traveler>().ToList();
            var tableReader = MockAzureCloudObjects(entities);
            var expected = entities.ToModel();

            // Act
            var result = await tableReader.GetAllAsync();

            // Assert
            Assert.IsInstanceOf<IEnumerable<Models.Traveler>>(result);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_ValidId_Traveler()
        {
            // Arrange
            var entity = CreateEntities(1);
            var tableReader = MockAzureCloudObjects(entity);
            var expected = entity.First().ToModel();

            // Act
            var result = await tableReader.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            Assert.IsInstanceOf<Models.Traveler>(result);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_IdNotFound_Null()
        {
            // Arrange
            var entity = CreateEntities(0);
            var tableReader = MockAzureCloudObjects(entity);

            // Act
            var result = await tableReader.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetByTypeAsync_ValidType_TravelerList()
        {
            // Arrange
            var entities = CreateEntities(5);
            var tableReader = MockAzureCloudObjects(entities);
            var expected = entities.ToModel();

            // Act
            var result = await tableReader.GetByTypeAsync(It.IsAny<TravelerType>());

            // Assert
            Assert.IsInstanceOf<IEnumerable<Models.Traveler>>(result);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_TypeNotFound_EmptyList()
        {
            // Arrange
            var entity = CreateEntities(0);
            var tableReader = MockAzureCloudObjects(entity);
            var expected = entity.ToModel();

            // Act
            var result = await tableReader.GetByTypeAsync(It.IsAny<TravelerType>());

            // Assert
            Assert.IsInstanceOf<IEnumerable<Models.Traveler>>(result);
            Assert.AreEqual(0, result.Count());
        }

        private IEnumerable<Entities.Traveler> CreateEntities(int quantity = 5)
        {
            return fixture.Build<Entities.Traveler>()
                            .With(e => e.RowKey, () => Guid.NewGuid().ToString())
                            .With(e => e.PartitionKey, () => TravelerType.Agent.ToString())
                            .CreateMany(quantity)
                            .ToList();
        }

        private TravelerTableReader MockAzureCloudObjects(IEnumerable<Entities.Traveler> resultset)
        {
            var cloudFactory = new Mock<IAzureCloudFactory>();
            var cloudTable = new Mock<CloudTableMock>();
            //var tableQuerySegment = new Mock<TableQuerySegment<Entities.Traveler>>(new List<Entities.Traveler>());

            var ctor = typeof(TableQuerySegment<Entities.Traveler>)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetParameters().Count() == 1);

            var tableQuerySegment = ctor.Invoke(new object[] { resultset }) as TableQuerySegment<Entities.Traveler>;

            cloudFactory.Setup(cf => cf.CreateCloudTable(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(cloudTable.Object);

            cloudTable.Setup(ct => ct.ExecuteQuerySegmentedAsync(
                It.IsAny<TableQuery<Entities.Traveler>>(),
                It.IsAny<TableContinuationToken>()))
                .ReturnsAsync(tableQuerySegment);

            var tableReader = new TravelerTableReader(logger, config, cloudFactory.Object);

            return tableReader;
        }
    }
}
