using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DotNet.Library.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNet.Library.Tests.Extensions
{
    [TestClass]
    public class HttpExtensionTests
    {
        private static JsonSerializerOptions GetOptions() =>
            new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        [TestMethod]
        public async Task EnsureObject_CaseInsensitiveFalse_NullProperites()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new TestDto
                    {
                        Name = "Hello World",
                        Id = 1
                    }, GetOptions())),
                StatusCode = HttpStatusCode.OK
            };

            // Act
            var result = await response.EnsureObject<TestDto>(false);

            // Assert
            result.Id.Should().Be(0);
            result.Name.Should().Be(null);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(null)]
        public async Task EnsureObject_CaseInsensitiveTrueOrNull_NonNullProperties(bool propertyNameCaseInsensitive)
        {
            // Arrange
            var dto = new TestDto
            {
                Name = "Hello World",
                Id = 1
            };
            var response = new HttpResponseMessage
                { Content = new StringContent(JsonSerializer.Serialize(dto)), StatusCode = HttpStatusCode.OK };

            // Act
            var result = await response.EnsureObject<TestDto>(propertyNameCaseInsensitive);

            // Assert
            result.Id.Should().Be(1);
            result.Name.Should().Be("Hello World");
        }

        [TestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.Forbidden)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.BadGateway)]
        [DataRow(HttpStatusCode.Conflict)]
        [DataRow(HttpStatusCode.Continue)]
        public async Task EnsureObject_NonSuccessStatusCode_ThrowsHttpRequestException(HttpStatusCode statusCode)
        {
            // Arrange
            var response = new HttpResponseMessage { Content = new StringContent(""), StatusCode = statusCode };

            // Act

            // Assert
            await Assert.ThrowsExactlyAsync<HttpRequestException>(async () => await response.EnsureObject<TestDto>());
        }

        [TestMethod]
        public async Task EnsureObject_EmptyResponseContent_ThrowsHttpRequestException()
        {
            // Arrange
            var response = new HttpResponseMessage { Content = new StringContent(""), StatusCode = HttpStatusCode.OK };

            // Act

            // Assert
            await Assert.ThrowsExactlyAsync<HttpRequestException>(async () => await response.EnsureObject<TestDto>());
        }

        private class TestDto
        {
            public string Name { get; init; }
            public int Id { get; init; }
        }
    }
}