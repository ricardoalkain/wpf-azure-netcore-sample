using System;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using TTMS.Common.DTO.Extensions;
using TTMS.Common.Enums;
using Entity = TTMS.Common.Entities;

namespace TTMS.Data.Tests.Extensions
{
    [TestFixture]
    public class EnityEntityExtensionsTest
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            this.fixture = new Fixture();
        }

        [Test]
        public void CreateRequest_TravelerEntity_TravelerRequest()
        {
            //Arrange
            var entity = fixture.Create<Entity.Traveler>();
            entity.RowKey = Guid.NewGuid().ToString();
            entity.PartitionKey = TravelerType.None.ToString();

            //Act
            var result = entity.CreateRequest();

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

        public void CreateRequest_NullRowKey_TravelerRequest()
        {
            //Arrange
            var entity = fixture.Create<Entity.Traveler>();
            entity.RowKey = null;
            entity.PartitionKey = TravelerType.None.ToString();

            //Act
            var result = entity.CreateRequest();

            //Assert
            Assert.IsNull(entity.RowKey);
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
