using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TTMS.Common.Abstractions;
using TTMS.Common.Enums;
using TTMS.Common.Models;
using TTMS.Web.Api.Core.Services;

namespace TTMS.Web.Api.Tests.Services
{
    [TestFixture]
    public class TravelerDbServiceTests
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            this.fixture = new Fixture();
        }

        [Test]
        public async Task GetAllAsync_ResultsFound_TravelerList()
        {
            // Arrange
            var (service, reader, _) = CreateTestObjects();
            var expected = fixture.CreateMany<Traveler>(5);
            reader.Setup(r => r.GetAllAsync()).ReturnsAsync(expected);

            // Act
            var result = await service.GetAllAsync().ConfigureAwait(false);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAllAsync_NoResults_EmptyList()
        {
            // Arrange
            var (service, reader, _) = CreateTestObjects();
            var expected = Enumerable.Empty<Traveler>();
            reader.Setup(r => r.GetAllAsync()).ReturnsAsync(expected);

            // Act
            var result = await service.GetAllAsync().ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetByIdAsync_ValidId_Traveler()
        {
            // Arrange
            var (service, reader, _) = CreateTestObjects();
            var expected = fixture.Create<Traveler>();
            reader.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expected);

            // Act
            var result = await service.GetByIdAsync(It.IsAny<Guid>()).ConfigureAwait(false);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByIdAsync_InexistentId_Null()
        {
            // Arrange
            var (service, reader, _) = CreateTestObjects();
            reader.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Traveler));

            // Act
            var result = await service.GetByIdAsync(It.IsAny<Guid>()).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetByTypeAsync_ValidType_TravelerList()
        {
            // Arrange
            var (service, reader, _) = CreateTestObjects();
            var expected = fixture.CreateMany<Traveler>(5);
            reader.Setup(r => r.GetByTypeAsync(It.IsAny<TravelerType>())).ReturnsAsync(expected);

            // Act
            var result = await service.GetByTypeAsync(It.IsAny<TravelerType>()).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetByTypeAsync_InexistentType_EmptyList()
        {
            // Arrange
            var (service, reader, _) = CreateTestObjects();
            var expected = Enumerable.Empty<Traveler>();
            reader.Setup(r => r.GetByTypeAsync(It.IsAny<TravelerType>())).ReturnsAsync(expected);

            // Act
            var result = await service.GetByTypeAsync(It.IsAny<TravelerType>()).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public async Task CreateAsync_ValidTraveler_Traveler()
        {
            // Arrange
            var (service, _, writer) = CreateTestObjects();
            var input = fixture.Create<Traveler>();
            writer.Setup(r => r.CreateAsync(It.IsAny<Traveler>())).ReturnsAsync(input);

            // Act
            var result = await service.CreateAsync(input).ConfigureAwait(false);

            // Assert
            result.Should().BeEquivalentTo(input);
        }

        [Test]
        public async Task CreateAsync_Null_Exception()
        {
            // Arrange
            var (service, _, writer) = CreateTestObjects();
            var input = fixture.Create<Traveler>();
            writer.Setup(r => r.CreateAsync(null)).ReturnsAsync(input);

            // Act / Assert
            await service.Invoking(async s => await s.CreateAsync(It.IsAny<Traveler>()).ConfigureAwait(false))
                .Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task CreateAsync_TravelerEmptyId_TravelerNewId()
        {
            // Arrange
            var (service, _, writer) = CreateTestObjects();
            var input = fixture.Create<Traveler>();
            input.Id = default;
            var expected = fixture.Create<Traveler>();
            writer.Setup(r => r.CreateAsync(input)).ReturnsAsync(expected);

            // Act
            var result = await service.CreateAsync(input).ConfigureAwait(false);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UpdateAsync_ValidTraveler_NoException()
        {
            // Arrange
            var (service, _, writer) = CreateTestObjects();

            // Act
            await service.UpdateAsync(It.IsAny<Traveler>()).ConfigureAwait(false);

            // Assert
            writer.Verify(w => w.UpdateAsync(It.IsAny<Traveler>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ValidId_NoException()
        {
            // Arrange
            var (service, _, writer) = CreateTestObjects();

            // Act
            await service.DeleteAsync(It.IsAny<Guid>()).ConfigureAwait(false);

            // Assert
            writer.Verify(w => w.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }


        private (TravelerDbService, Mock<ITravelerReader>, Mock<ITravelerWriter>) CreateTestObjects()
        {
            var logger = new Mock<ILogger<TravelerDbService>>();
            var reader = new Mock<ITravelerReader>();
            var writer = new Mock<ITravelerWriter>();
            var service = new TravelerDbService(logger.Object, reader.Object, writer.Object);

            return (service, reader, writer);
        }
    }
}
