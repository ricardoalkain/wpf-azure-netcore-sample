using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TTMS.Common.Abstractions;
using TTMS.Common.Enums;
using TTMS.Common.Models;
using TTMS.UI.Services;

namespace TTMS.UI.Tests.Services
{
    [TestFixture]
    public class TravelerServiceTests
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
            var expected = fixture.Create<Traveler>();
            writer.Setup(r => r.CreateAsync(It.IsAny<Traveler>())).ReturnsAsync(expected);

            // Act
            var result = await service.CreateAsync(It.IsAny<Traveler>()).ConfigureAwait(false);

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


        private (TravelerService, Mock<ITravelerReader>, Mock<ITravelerWriter>) CreateTestObjects()
        {
            var reader = new Mock<ITravelerReader>();
            var writer = new Mock<ITravelerWriter>();
            var service = new TravelerService(reader.Object, writer.Object);

            return (service, reader, writer);
        }
    }
}
