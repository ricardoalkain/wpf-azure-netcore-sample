using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using TTMS.Common.Entities.Extensions;
using Model = TTMS.Common.Models;
using Entity = TTMS.Common.Entities;
using System;
using System.Linq;
using TTMS.Common.Enums;

namespace TTMS.Common.Tests.Entities
{
    [TestFixture]
    public class EntityModelMappingTest
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
            var model = fixture.Create<Model.Traveler>();

            //Act
            var result = model.ToEntity();

            //Assert
            Assert.AreEqual(model.Id.ToString(), result.RowKey);
            Assert.AreEqual(model.Type.ToString(), result.PartitionKey);
            Assert.AreEqual((int)model.DeviceModel, result.DeviceModel);
            Assert.AreEqual((int)model.Status, result.Status);

            Assert.AreEqual(model.Alias, result.Alias);
            Assert.AreEqual(model.BirthDate, result.BirthDate);
            Assert.AreEqual(model.BirthLocation, result.BirthLocation);
            Assert.AreEqual(model.BirthTimelineId, result.BirthTimelineId);
            Assert.AreEqual(model.LastDateTime, result.LastDateTime);
            Assert.AreEqual(model.LastLocation, result.LastLocation);
            Assert.AreEqual(model.LastTimelineId, result.LastTimelineId);
            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.Picture, result.Picture);
            Assert.AreEqual(model.Skills, result.Skills);
        }

        [Test]
        public void ToEntity_ListOfTravelerModels_ListOfTravelerEntities()
        {
            //Arrange
            var models = fixture.CreateMany<Model.Traveler>(5);

            //Act
            var results = models.ToEntity();

            //Assert
            foreach (var result in results)
            {
                var model = models.FirstOrDefault(m => m.Id.ToString() == result.RowKey);

                if (model == null)
                {
                    Assert.Fail("The list of items is different: ID not found");
                }

                Assert.AreEqual(model.Id.ToString(), result.RowKey);
                Assert.AreEqual(model.Type.ToString(), result.PartitionKey);
                Assert.AreEqual((int)model.DeviceModel, result.DeviceModel);
                Assert.AreEqual((int)model.Status, result.Status);

                Assert.AreEqual(model.Alias, result.Alias);
                Assert.AreEqual(model.BirthDate, result.BirthDate);
                Assert.AreEqual(model.BirthLocation, result.BirthLocation);
                Assert.AreEqual(model.BirthTimelineId, result.BirthTimelineId);
                Assert.AreEqual(model.LastDateTime, result.LastDateTime);
                Assert.AreEqual(model.LastLocation, result.LastLocation);
                Assert.AreEqual(model.LastTimelineId, result.LastTimelineId);
                Assert.AreEqual(model.Name, result.Name);
                Assert.AreEqual(model.Picture, result.Picture);
                Assert.AreEqual(model.Skills, result.Skills);
            }
        }

        [Test]
        public void ToModel_TravelerEntity_TravelerModel()
        {
            //Arrange
            var entity = fixture.Build<Entity.Traveler>()
                                .With(e => e.RowKey, () => Guid.NewGuid().ToString())
                                .With(e => e.PartitionKey, () => TravelerType.Agent.ToString())
                                .Create();

            //Act
            var result = entity.ToModel();

            //Assert
            Assert.AreEqual(entity.RowKey, result.Id.ToString());
            Assert.AreEqual(entity.PartitionKey, result.Type.ToString());
            Assert.AreEqual(entity.DeviceModel, (int)result.DeviceModel);
            Assert.AreEqual(entity.Status, (int)result.Status);

            Assert.AreEqual(entity.Alias, result.Alias);
            Assert.AreEqual(entity.BirthDate, result.BirthDate);
            Assert.AreEqual(entity.BirthLocation, result.BirthLocation);
            Assert.AreEqual(entity.BirthTimelineId, result.BirthTimelineId);
            Assert.AreEqual(entity.LastDateTime, result.LastDateTime);
            Assert.AreEqual(entity.LastLocation, result.LastLocation);
            Assert.AreEqual(entity.LastTimelineId, result.LastTimelineId);
            Assert.AreEqual(entity.Name, result.Name);
            Assert.AreEqual(entity.Picture, result.Picture);
            Assert.AreEqual(entity.Skills, result.Skills);
        }

        [Test]
        public void ToModel_ListOfTravelerEntities_ListOfTravelerModels()
        {
            //Arrange
            var entities = fixture.Build<Entity.Traveler>()
                                .With(e => e.RowKey, () => Guid.NewGuid().ToString())
                                .With(e => e.PartitionKey, () => TravelerType.Agent.ToString())
                                .CreateMany(5);

            //Act
            var results = entities.ToModel();

            //Assert
            foreach (var result in results)
            {
                var entity = entities.FirstOrDefault(e => e.RowKey == result.Id.ToString());

                if (entity == null)
                {
                    Assert.Fail("The list of items is different: ID not found");
                }

                Assert.AreEqual(entity.RowKey, result.Id.ToString());
                Assert.AreEqual(entity.PartitionKey, result.Type.ToString());
                Assert.AreEqual(entity.DeviceModel, (int)result.DeviceModel);
                Assert.AreEqual(entity.Status, (int)result.Status);

                Assert.AreEqual(entity.Alias, result.Alias);
                Assert.AreEqual(entity.BirthDate, result.BirthDate);
                Assert.AreEqual(entity.BirthLocation, result.BirthLocation);
                Assert.AreEqual(entity.BirthTimelineId, result.BirthTimelineId);
                Assert.AreEqual(entity.LastDateTime, result.LastDateTime);
                Assert.AreEqual(entity.LastLocation, result.LastLocation);
                Assert.AreEqual(entity.LastTimelineId, result.LastTimelineId);
                Assert.AreEqual(entity.Name, result.Name);
                Assert.AreEqual(entity.Picture, result.Picture);
                Assert.AreEqual(entity.Skills, result.Skills);
            }
        }
    }
}
