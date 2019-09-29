using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using TTMS.Data.Extensions;

namespace TTMS.Data.Tests.Extensions
{
    [TestFixture]
    public class EnityModelExtensionsTest
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            this.fixture = new Fixture();
        }

        [Test]
        public void ToEntity_TravelerModel_TravelerEntity()
        {
            //Arrange
            var model = fixture.Create<Models.Traveler>();

            //Act
            var result = model.ToEntity();

            //Assert
            result.Should().BeEquivalentTo(model);
        }

        [Test]
        public void ToEntity_ListOfTravelerModels_ListOfTravelerEntities()
        {
            //Arrange
            var model = fixture.CreateMany<Models.Traveler>(5);

            //Act
            var result = model.ToEntity();

            //Assert
            result.Should().BeEquivalentTo(model);
        }

        [Test]
        public void ToModel_TravelerEntity_TravelerModel()
        {
            //Arrange
            var entity = fixture.Create<Entities.Traveler>();

            //Act
            var result = entity.ToModel();

            //Assert
            result.Should().BeEquivalentTo(entity);
        }

        [Test]
        public void ToModel_ListOfTravelerEntities_ListOfTravelerModels()
        {
            //Arrange
            var entities = fixture.CreateMany<Entities.Traveler>(5);

            //Act
            var result = entities.ToModel();

            //Assert
            result.Should().BeEquivalentTo(entities);
        }
    }
}
