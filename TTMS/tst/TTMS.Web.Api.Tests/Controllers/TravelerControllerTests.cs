using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TTMS.Common.DTO;
using TTMS.Common.DTO.Extensions;
using TTMS.Common.Enums;
using TTMS.Common.Models;
using TTMS.Web.Api.Controllers;
using TTMS.Web.Api.Core.Services;

namespace TTMS.Web.Api.Tests.Controllers
{
    public class TravelerControllerTests
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            this.fixture = new Fixture();
        }

        [Test]
        public async Task Get_NoInput_OkTravelerList()
        {
            // Arrange
            var (controller, service) = CreateTestObjects();
            var travelers = fixture.CreateMany<Traveler>(5);

            service.Setup(s => s.GetAllAsync()).ReturnsAsync(travelers);

            // Act
            var result = await controller.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (result as OkObjectResult);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(travelers);
        }

        [Test]
        public async Task GetByType_ValidType_OkTravelerList()
        {
            // Arrange
            var (controller, service) = CreateTestObjects();
            var travelers = fixture.CreateMany<Traveler>(5);

            service.Setup(s => s.GetByTypeAsync(It.IsAny<TravelerType>())).ReturnsAsync(travelers);

            // Act
            var result = await controller.GetBytype(It.IsAny<TravelerType>());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (result as OkObjectResult);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(travelers);
        }

        [Test]
        public async Task GetById_ValidId_OkTraveler()
        {
            // Arrange
            var (controller, service) = CreateTestObjects();
            var traveler = fixture.Create<Traveler>();
            var expected = traveler.CreateResponse();

            service.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(traveler);

            // Act
            var result = await controller.GetById(It.IsAny<Guid>());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (result as OkObjectResult);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetById_InexistentId_NotFound()
        {
            // Arrange
            var (controller, service) = CreateTestObjects();

            service.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Traveler));

            // Act
            var result = await controller.GetById(It.IsAny<Guid>());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Post_ValidRequest_CreatedLink()
        {
            // Arrange
            var (controller, service) = CreateTestObjects();
            var traveler = fixture.Create<Traveler>();
            var request = traveler.CreateRequest();
            service.Setup(s => s.CreateAsync(It.IsAny<Traveler>())).ReturnsAsync(traveler);

            // Act
            var result = await controller.Post(request);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = (result as CreatedResult);
            createdResult.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(traveler);
        }

        [Test]
        public async Task Put_ValidRequest_Ok()
        {
            // Arrange
            var (controller, service) = CreateTestObjects();
            var traveler = fixture.Create<Traveler>();
            var request = traveler.CreateRequest();

            // Act
            var result = await controller.Put(traveler.Id, request);

            // Assert
            result.Should().BeOfType<OkResult>();
            service.Verify(s => s.UpdateAsync(It.IsAny<Traveler>()), Times.Once);
        }

        [Test]
        public async Task Post_NullRequest_BadRequest()
        {
            // Arrange
            var (controller, _) = CreateTestObjects();

            // Act
            var result = await controller.Post(null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Put_DifferentId_BadRequest()
        {
            // Arrange
            var (controller, _) = CreateTestObjects();
            var request = fixture.Create<TravelerRequest>();

            // Act
            var result = await controller.Put(Guid.NewGuid(), request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Put_NullRequest_BadRequest()
        {
            // Arrange
            var (controller, _) = CreateTestObjects();

            // Act
            var result = await controller.Put(Guid.NewGuid(), null);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Delete_AnyId_Ok()
        {
            // Arrange
            var (controller, _) = CreateTestObjects();

            // Act
            var result = await controller.Delete(It.IsAny<Guid>());

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        private (TravelerController, Mock<ITravelerDbService>) CreateTestObjects()
        {
            var logger = new Mock<ILogger<TravelerController>>();
            var service = new Mock<ITravelerDbService>();
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(u => u.Action(It.IsAny<UrlActionContext>())).Returns("http://test/test");

            var controller = new TravelerController(logger.Object, service.Object);
            controller.Url = urlHelper.Object;
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            return (controller, service);
        }
    }
}