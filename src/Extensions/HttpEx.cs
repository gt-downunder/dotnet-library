using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace DotNet.Library.Extensions
{
    public static class HttpEx
    {
        private static readonly JsonSerializerOptions Options = new();

        /// <summary>
        ///     <para>Deserialize a <see cref="HttpResponseMessage"/> into the specified <typeparamref name="TObject"/> type.</para>
        ///     <para>Throws a <see cref="HttpRequestException"/> if the <paramref name="response"/> <see cref="HttpContent"/> is null, empty, or contains only whitespace.</para>
        /// </summary>
        /// <typeparam name="TObject">What to deserialize the response to</typeparam>
        /// <param name="response">The response from the service</param>
        /// <param name="propertyNameCaseInsensitive">Indicates whether a property's name uses a case-insensitive comparison during deserialization.</param>
        public static async Task<TObject?> EnsureObject<TObject>(this HttpResponseMessage response, bool propertyNameCaseInsensitive = true)
        {
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (responseBody.IsNullOrEmpty())
            {
                throw new HttpRequestException($"{nameof(response.Content)} is null, empty, or whitespace");
            }

            Options.PropertyNameCaseInsensitive = propertyNameCaseInsensitive;

            return JsonSerializer.Deserialize<TObject>(responseBody, Options);
        }

        /// <summary>
        /// Send a POST request to the <paramref name="requestUri"/> as an asynchronous operation with serialized <typeparamref name="TObject"/> content.
        /// </summary>
        /// <typeparam name="TObject">The type of object to serialize.</typeparam>
        /// <param name="httpClient"></param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="data">The <typeparamref name="TObject"/> data to serialize.</param>
        /// <param name="token">An optional cancellation token.</param>
        public static async Task<HttpResponseMessage> PostAsJsonAsync<TObject>(this HttpClient httpClient, string requestUri, TObject data, CancellationToken token = default)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(requestUri, content, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Send a PUT request to the <paramref name="requestUri"/> as an asynchronous operation with serialized <typeparamref name="TObject"/> content.
        /// </summary>
        /// <typeparam name="TObject">The type of object to serialize.</typeparam>
        /// <param name="httpClient"></param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="data">The <typeparamref name="TObject"/> data to serialize.</param>
        /// <param name="token">An optional cancellation token.</param>
        public static async Task<HttpResponseMessage> PutAsJsonAsync<TObject>(this HttpClient httpClient, string requestUri, TObject data, CancellationToken token = default)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            return await httpClient.PutAsync(requestUri, content, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve a raw string from the <see cref="HttpRequest.Body"/> stream
        /// </summary>
        /// <param name="request"></param>
        /// <param name="encoding">Optional character encoding, defaults to UTF8</param>
        public static async Task<string> GetRawBodyAsync(this HttpRequest request, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            using StreamReader reader = new(request.Body, encoding);
            return await reader.ReadToEndAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves a raw byte array from the <see cref="HttpRequest.Body"/> stream
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token">An optional cancellation token.</param>
        public static async Task<byte[]> GetRawBodyAsync(this HttpRequest request, CancellationToken token = default)
        {
            await using MemoryStream stream = new();
            // 81920 is the default size and is 80KB (or 1024B * 80)
            var bufferSize = Math.Min(81920, stream.Length).ToNullableInteger() ?? 0;

            await request.Body.CopyToAsync(stream, bufferSize, token).ConfigureAwait(false);
            return stream.ToArray();
        }
    }
}
