using System.Text;
using System.Text.Json;
using Grondo.Utilities;
using Microsoft.AspNetCore.Http;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for HTTP-related types such as <see cref="HttpResponseMessage"/> and <see cref="HttpRequest"/>.
    /// </summary>
    public static class HttpEx
    {
        extension(HttpResponseMessage response)
        {
            /// <summary>
            ///     <para>Deserialize a <see cref="HttpResponseMessage"/> into the specified <typeparamref name="TObject"/> type.</para>
            ///     <para>Throws a <see cref="HttpRequestException"/> if the <paramref name="response"/> <see cref="HttpContent"/> is null, empty, or contains only whitespace.</para>
            /// </summary>
            /// <typeparam name="TObject">What to deserialize the response to</typeparam>
            /// <param name="propertyNameCaseInsensitive">Indicates whether a property's name uses a case-insensitive comparison during deserialization.</param>
            public async Task<TObject?> EnsureObjectAsync<TObject>(bool propertyNameCaseInsensitive = true)
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(responseBody))
                {
                    throw new HttpRequestException($"{nameof(response.Content)} is null, empty, or whitespace");
                }

                JsonSerializerOptions options = propertyNameCaseInsensitive ? JsonDefaults.CaseInsensitive : JsonDefaults.Default;

                return JsonSerializer.Deserialize<TObject>(responseBody, options);
            }
        }

        extension(HttpRequest request)
        {
            /// <summary>
            /// Retrieve a raw string from the <see cref="HttpRequest.Body"/> stream
            /// </summary>
            /// <param name="encoding">Optional character encoding, defaults to UTF8</param>
            public async Task<string> GetRawBodyAsync(Encoding? encoding = null)
            {
                encoding ??= Encoding.UTF8;

                using StreamReader reader = new(request.Body, encoding);
                return await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            /// <summary>
            /// Retrieves a raw byte array from the <see cref="HttpRequest.Body"/> stream
            /// </summary>
            /// <param name="token">An optional cancellation token.</param>
            public async Task<byte[]> GetRawBodyAsync(CancellationToken token = default)
            {
                await using MemoryStream stream = new();
                await request.Body.CopyToAsync(stream, token).ConfigureAwait(false);
                return stream.ToArray();
            }
        }
    }
}
