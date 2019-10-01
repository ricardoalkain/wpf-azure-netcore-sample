using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using TTMS.Common.Abstractions;
using TTMS.Common.Messages;
using TTMS.Common.Models;
using TTMS.Messaging.Consumers;

namespace Tests
{
    public class TravelerConsumerTests
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            this.fixture = new Fixture();
        }

        [Test]
        public async Task ProcessMessage_CreateMessage_ExecCreate()
        {
            // Arrange
            var (consumer, writer) = CreateTestObjects();
            var message = fixture.Build<TravelerMessage>()
                                 .With(m => m.Type, MessageType.Create)
                                 .Create();
            // Act
            await consumer.ProcessMessageAsync(JsonConvert.SerializeObject(message));

            // Assert
            writer.Verify(w => w.CreateAsync(It.IsAny<Traveler>()), Times.Once);
            writer.Verify(w => w.UpdateAsync(It.IsAny<Traveler>()), Times.Never);
            writer.Verify(w => w.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public async Task ProcessMessage_UpdateMessage_ExecUpdate()
        {
            // Arrange
            var (consumer, writer) = CreateTestObjects();
            var message = fixture.Build<TravelerMessage>()
                                 .With(m => m.Type, MessageType.Update)
                                 .Create();
            // Act
            await consumer.ProcessMessageAsync(JsonConvert.SerializeObject(message));

            // Assert
            writer.Verify(w => w.UpdateAsync(It.IsAny<Traveler>()), Times.Once);
            writer.Verify(w => w.CreateAsync(It.IsAny<Traveler>()), Times.Never);
            writer.Verify(w => w.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public async Task ProcessMessage_DeleteMessage_ExecDelete()
        {
            // Arrange
            var (consumer, writer) = CreateTestObjects();
            var message = fixture.Build<TravelerMessage>()
                                 .With(m => m.Type, MessageType.Delete)
                                 .Create();
            // Act
            await consumer.ProcessMessageAsync(JsonConvert.SerializeObject(message));

            // Assert
            writer.Verify(w => w.DeleteAsync(It.IsAny<Guid>()), Times.Once);
            writer.Verify(w => w.CreateAsync(It.IsAny<Traveler>()), Times.Never);
            writer.Verify(w => w.UpdateAsync(It.IsAny<Traveler>()), Times.Never);
        }

        [Test]
        public void ProcessMessage_UnknownMessage_Exception()
        {
            // Arrange
            var (consumer, writer) = CreateTestObjects();
            var message = fixture.Build<TravelerMessage>()
                                 .With(m => m.Type, (MessageType)123456)
                                 .Create();
            // Act / Assert
            Assert.ThrowsAsync<NotImplementedException>(async () => await consumer.ProcessMessageAsync(JsonConvert.SerializeObject(message)));

            writer.Verify(w => w.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            writer.Verify(w => w.CreateAsync(It.IsAny<Traveler>()), Times.Never);
            writer.Verify(w => w.UpdateAsync(It.IsAny<Traveler>()), Times.Never);
        }

        [Test]
        public void ProcessMessage_InvalidMessage_Exception()
        {
            // Arrange
            var (consumer, writer) = CreateTestObjects();
            var message = fixture.Create<string>();

            // Act / Assert
            Assert.CatchAsync<JsonException>(async () => await consumer.ProcessMessageAsync(JsonConvert.SerializeObject(message)));

            writer.Verify(w => w.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            writer.Verify(w => w.CreateAsync(It.IsAny<Traveler>()), Times.Never);
            writer.Verify(w => w.UpdateAsync(It.IsAny<Traveler>()), Times.Never);
        }

        private (TravelerConsumer, Mock<ITravelerWriter>) CreateTestObjects()
        {
            var queueClient = new Mock<IQueueClient>();
            var logger = new Mock<ILogger>();
            var travelerWriter = new Mock<ITravelerWriter>();

            var consumer = new TravelerConsumer(logger.Object, queueClient.Object, travelerWriter.Object);

            return (consumer, travelerWriter);
        }
    }
}