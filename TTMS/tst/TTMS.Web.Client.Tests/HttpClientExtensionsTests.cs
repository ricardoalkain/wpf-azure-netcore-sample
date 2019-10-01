using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TTMS.Common.Models;

namespace TTMS.Web.Client.Tests
{
    public class HttpClientExtensionsTests
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            this.fixture = new Fixture();
        }

        [Test]
        public async Task ReadAsync_SuccessfulResponse_Object()
        {
            // Arrange
            var traveler = fixture.Create<Traveler>();
            var httpContent = new Mock<HttpContent>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = httpContent.Object
            };

            // Act / Assert
            await response.Invoking(r => r.ReadAsync<Traveler>())
                .Should().NotThrowAsync();
        }

        [Test]
        public async Task ReadAsync_NotSuccessfulResponse_Exception()
        {
            // Arrange
            var traveler = fixture.Create<Traveler>();
            var httpContent = new Mock<HttpContent>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = httpContent.Object
            };

            // Act / Assert
            await response.Invoking(r => r.ReadAsync<Traveler>())
                .Should().ThrowAsync<HttpRequestException>();
        }

        [Test]
        public void CheckResult_SuccessfulResponse_Object()
        {
            // Arrange
            var traveler = fixture.Create<Traveler>();
            var httpContent = new Mock<HttpContent>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = httpContent.Object
            };

            // Act / Assert
            response.Invoking(r => r.CheckResult())
                .Should().NotThrow();
        }

        [Test]
        public void CheckResult_NotSuccessfulResponse_Exception()
        {
            // Arrange
            var traveler = fixture.Create<Traveler>();
            var httpContent = new Mock<HttpContent>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = httpContent.Object
            };

            // Act / Assert
            response.Invoking(r => r.CheckResult())
                .Should().Throw<HttpRequestException>();
        }

        // TODO: Mock HttpContent internal stream to check correct object deserialization
    }
}