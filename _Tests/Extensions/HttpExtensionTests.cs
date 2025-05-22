using DotNet.Library.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNet.Library.Tests.Extensions
{
    [TestClass]
    public class HttpExtensionTests
    {
        private static JsonSerializerOptions GetOptions() => new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

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
                StatusCode = System.Net.HttpStatusCode.OK
            };

            // Act
            var result = await response.EnsureObject<TestDto>(false);

            // Assert
            result.Id.Should().Be(default);
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
            var response = new HttpResponseMessage { Content = new StringContent(JsonSerializer.Serialize(dto)), StatusCode = System.Net.HttpStatusCode.OK };

            // Act
            var result = await response.EnsureObject<TestDto>(propertyNameCaseInsensitive);

            // Assert
            result.Id.Should().Be(1);
            result.Name.Should().Be("Hello World");
        }

        [TestMethod]
        [DataRow(System.Net.HttpStatusCode.BadRequest)]
        [DataRow(System.Net.HttpStatusCode.Unauthorized)]
        [DataRow(System.Net.HttpStatusCode.Forbidden)]
        [DataRow(System.Net.HttpStatusCode.InternalServerError)]
        [DataRow(System.Net.HttpStatusCode.NotFound)]
        [DataRow(System.Net.HttpStatusCode.BadGateway)]
        [DataRow(System.Net.HttpStatusCode.Conflict)]
        [DataRow(System.Net.HttpStatusCode.Continue)]
        public async Task EnsureObject_NonSuccessStatusCode_ThrowsHttpRequestException(System.Net.HttpStatusCode statusCode)
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
            var response = new HttpResponseMessage { Content = new StringContent(""), StatusCode = System.Net.HttpStatusCode.OK };

            // Act

            // Assert
            await Assert.ThrowsExactlyAsync<HttpRequestException>(async () => await response.EnsureObject<TestDto>());
        }

        private class TestDto
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
    }
}
