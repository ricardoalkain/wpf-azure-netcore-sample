using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using TTMS.Data.Entities;
using TTMS.Data.Extensions;
using TTMS.Data.Repositories;

namespace TTMS.Data.Tests.Repositories
{
    [TestFixture]
    public class TravelerFileRepositoryTests
    {
        private const string dataFilePath = @"c:\home\data\mock.json";

        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            this.fixture = new Fixture();
        }

        [Test]
        public async Task GetAllAsync_ValidFile_ListOfTravelers()
        {
            //Arrange
            var expected = fixture.CreateMany<Traveler>(3);
            var fileSystem = CreateReposistoryTestFile(dataFilePath, expected);
            var logger = new Mock<ILogger>();

            var repo = new TravelerFileRepository(logger.Object, fileSystem, dataFilePath);

            //Act
            var result = await repo.GetAllAsync().ConfigureAwait(false);

            //Assert
            expected.Should().BeEquivalentTo(result);
        }

        [Test]
        public void GetAllAsync_InvalidFile_Exception()
        {
            //Arrange
            var fileSystem = CreateReposistoryTestFile(dataFilePath, "invalid json content");
            var logger = new Mock<ILogger>();

            var repo = new TravelerFileRepository(logger.Object, fileSystem, dataFilePath);

            //Act / Assert
            Assert.CatchAsync<JsonException>(async () => await repo.GetAllAsync().ConfigureAwait(false));
        }

        [Test]
        public void GetAllAsync_InvalidPath_Exception()
        {
            //Arrange
            var fileSystem = CreateReposistoryTestFile(fixture.Create<string>(), "");
            var logger = new Mock<ILogger>();

            var repo = new TravelerFileRepository(logger.Object, fileSystem, dataFilePath);

            //Act / Assert
            Assert.ThrowsAsync<FileNotFoundException>(async () => await repo.GetAllAsync().ConfigureAwait(false));
        }

        [Test]
        public async Task GetByIdAsync_ValidId_Entity()
        {
            //Arrange
            var fileContent = fixture.CreateMany<Traveler>(3);
            var expected = fileContent.First();
            var fileSystem = CreateReposistoryTestFile(dataFilePath, fileContent);
            var logger = new Mock<ILogger>();

            var repo = new TravelerFileRepository(logger.Object, fileSystem, dataFilePath);

            //Act
            var result = await repo.GetByIdAsync(expected.Id).ConfigureAwait(false);

            //Assert
            expected.Should().BeEquivalentTo(result);
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_Null()
        {
            //Arrange
            var fileContent = fixture.CreateMany<Traveler>(3);
            var fileSystem = CreateReposistoryTestFile(dataFilePath, fileContent);
            var logger = new Mock<ILogger>();

            var repo = new TravelerFileRepository(logger.Object, fileSystem, dataFilePath);

            //Act
            var result = await repo.GetByIdAsync(Guid.NewGuid()).ConfigureAwait(false);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task DeteleAsync_ValidId_NoErrors()
        {
            //Arrange
            var fileContent = fixture.CreateMany<Traveler>(5).ToList();
            var idToDelete = fileContent[2].Id;
            var fileSystem = CreateReposistoryTestFile(dataFilePath, fileContent);
            var logger = new Mock<ILogger>();

            var repo = new TravelerFileRepository(logger.Object, fileSystem, dataFilePath);

            //Act
            var itemToDelete = await repo.GetByIdAsync(idToDelete).ConfigureAwait(false); // Ensures it was in the file previously
            await repo.DeleteAsync(itemToDelete.Id).ConfigureAwait(false);
            var result = await repo.GetByIdAsync(itemToDelete.Id).ConfigureAwait(false);

            //Assert
            itemToDelete.Should().NotBeNull();
            result.Should().BeNull();
        }

        [Test]
        public void InsertOrUpdateAsync_NullValue_Exception()
        {
            //Arrange
            var fileContent = fixture.CreateMany<Traveler>(3).ToList();
            var fileSystem = CreateReposistoryTestFile(dataFilePath, fileContent);
            var logger = new Mock<ILogger>();

            var repo = new TravelerFileRepository(logger.Object, fileSystem, dataFilePath);

            //Act
            Func<Task> act = async () => await repo.InsertOrReplaceAsync(null).ConfigureAwait(false);

            //Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public async Task InsertOrUpdateAsync_NewId_InsertEntity()
        {
            //Arrange
            var fileContent = fixture.CreateMany<Traveler>(3).ToList();
            var expected = fixture.Create<Traveler>(); // New Id
            var fileSystem = CreateReposistoryTestFile(dataFilePath, fileContent);
            var logger = new Mock<ILogger>();

            var repo = new TravelerFileRepository(logger.Object, fileSystem, dataFilePath);

            //Act
            await repo.InsertOrReplaceAsync(expected).ConfigureAwait(false);
            var result = await repo.GetByIdAsync(expected.Id).ConfigureAwait(false);
            var newContent = await repo.GetAllAsync().ConfigureAwait(false);

            //Assert
            expected.Should().NotBeNull();
            expected.Should().BeEquivalentTo(result);
            newContent.Should().HaveCount(fileContent.Count() + 1);
        }

        [Test]
        public async Task InsertOrUpdateAsync_ExistingId_UpdatesEntity()
        {
            //Arrange
            var fileContent = fixture.CreateMany<Traveler>(3).ToList();
            var fileSystem = CreateReposistoryTestFile(dataFilePath, fileContent);
            var logger = new Mock<ILogger>();

            var repo = new TravelerFileRepository(logger.Object, fileSystem, dataFilePath);

            //Act
            var id = fileContent[2].Id;
            var expected = fixture.Create<Traveler>();
            expected.Id = id; // restore original id
            await repo.InsertOrReplaceAsync(expected).ConfigureAwait(false);
            var result = await repo.GetByIdAsync(expected.Id).ConfigureAwait(false);

            //Assert
            expected.Should().NotBeNull();
            expected.Should().BeEquivalentTo(result);
            expected.Should().NotBeEquivalentTo(fileContent[2]);
        }







        private IFileSystem CreateReposistoryTestFile(string fileName, IEnumerable<Traveler> fileContent)
        {
            return CreateReposistoryTestFile(fileName, JsonConvert.SerializeObject(fileContent));
        }

        private IFileSystem CreateReposistoryTestFile(string fileName, string fileContent)
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile(fileName, new MockFileData(fileContent, Encoding.UTF8));

            return fileSystem;
        }
    }
}
