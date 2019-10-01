using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TTMS.Common.Enums;
using TTMS.Data.Abstractions;
using TTMS.Data.Azure;
using TTMS.Data.Tests.Mocks;
using Entities = TTMS.Common.Entities;
using Models = TTMS.Common.Models;

namespace TTMS.Data.Tests.Azure
{
    [TestFixture]
    public class TravelerTableWriterTests
    {
        private ILogger<TravelerTableWriter> logger;
        private IConfiguration config;
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger<TravelerTableWriter>>().Object;
            config = new Mock<IConfiguration>().Object;
            fixture = new Fixture();
        }

        [Test]
        public async Task CreateAsync_ValidTraveler_Traveler()
        {
            // Arrange
            var writer = MockAzureCloudObjects();
            var expected = fixture.Create<Models.Traveler>();

            // Act
            var result = await writer.CreateAsync(expected);

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateAsync_EmptyId_TravelerWithNewId()
        {
            // Arrange
            var writer = MockAzureCloudObjects();
            var newTraveler = fixture.Create<Models.Traveler>();
            newTraveler.Id = default;

            // Act
            var result = await writer.CreateAsync(newTraveler);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(default(Guid), result.Id);
        }

        [Test]
        public async Task CreateAsync_NullTraveler_Exception()
        {
            // Arrange
            var writer = MockAzureCloudObjects();

            // Act / Assert
            await writer.Invoking(w => w.CreateAsync(null))
                        .Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task UpdateAsync_ValidTraveler_NoException()
        {
            // Arrange
            var writer = MockAzureCloudObjects();
            var expected = fixture.Create<Models.Traveler>();

            // Act
            await writer.UpdateAsync(expected);
        }

        [Test]
        public async Task UpdateAsync_NullTraveler_Exception()
        {
            // Arrange
            var writer = MockAzureCloudObjects();

            // Act / Assert
            await writer.Invoking(w => w.UpdateAsync(null))
                        .Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task DeleteAsync_ValidId_NoException()
        {
            // Arrange
            var entityToDelete = CreateEntities(1);
            var writer = MockAzureCloudObjects(entityToDelete);

            // Act
            await writer.DeleteAsync(It.IsAny<Guid>());
        }

        [Test]
        public async Task DeleteAsync_InexistentId_NoException()
        {
            // Arrange
            var entityToDelete = CreateEntities(0);
            var writer = MockAzureCloudObjects(entityToDelete);

            // Act
            await writer.DeleteAsync(It.IsAny<Guid>());
        }


        private TravelerTableWriter MockAzureCloudObjects()
        {
            var cloudFactory = new Mock<IAzureCloudFactory>();
            var cloudTable = new Mock<CloudTableMock>();

            cloudFactory.Setup(cf => cf.CreateCloudTable(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(cloudTable.Object);

            var tableReader = new TravelerTableWriter(logger, config, cloudFactory.Object);

            return tableReader;
        }

        private TravelerTableWriter MockAzureCloudObjects(IEnumerable<Entities.Traveler> resultset)
        {
            var cloudFactory = new Mock<IAzureCloudFactory>();
            var cloudTable = new Mock<CloudTableMock>();

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

            var tableReader = new TravelerTableWriter(logger, config, cloudFactory.Object);

            return tableReader;
        }

        private IEnumerable<Entities.Traveler> CreateEntities(int quantity = 5)
        {
            return fixture.Build<Entities.Traveler>()
                            .With(e => e.RowKey, () => Guid.NewGuid().ToString())
                            .With(e => e.PartitionKey, () => TravelerType.Agent.ToString())
                            .CreateMany(quantity)
                            .ToList();
        }
    }
}
