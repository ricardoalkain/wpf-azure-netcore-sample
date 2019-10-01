using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using TTMS.Common.DTO.Extensions;
using Model = TTMS.Common.Models;

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
        public void CreateRequest_TravelerModel_TravelerRequest()
        {
            //Arrange
            var model = fixture.Create<Model.Traveler>();

            //Act
            var result = model.CreateRequest();

            //Assert
            result.Should().BeEquivalentTo(model);
        }

        [Test]
        public void CreateRequest_TravelerModelEmptyId_TravelerRequestNullId()
        {
            //Arrange
            var model = fixture.Create<Model.Traveler>();
            model.Id = default;

            //Act
            var result = model.CreateRequest();

            //Assert
            result.Should().BeEquivalentTo(model, op => op.Excluding(m => m.Id));
            result.Id.Should().BeNull();
        }

        [Test]
        public void CreateResponse_TravelerModel_TravelerResponse()
        {
            //Arrange
            var model = fixture.Create<Model.Traveler>();

            //Act
            var result = model.CreateResponse();

            //Assert
            result.Should().BeEquivalentTo(model);
        }

        [Test]
        public void CreateResponse_TravelerModelList_TravelerResponseList()
        {
            //Arrange
            var model = fixture.CreateMany<Model.Traveler>(5);

            //Act
            var result = model.CreateResponse();

            //Assert
            result.Should().BeEquivalentTo(model);
        }
    }
}
