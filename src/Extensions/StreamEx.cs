using System.Text;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Stream"/>.
    /// </summary>
    public static class StreamEx
    {
        extension(Stream stream)
        {
            /// <summary>
            /// Reads the entire stream into a byte array asynchronously.
            /// The stream position is reset to the beginning before reading.
            /// </summary>
            /// <param name="cancellationToken">An optional cancellation token.</param>
            /// <returns>A byte array containing the stream's contents.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the stream is <c>null</c>.</exception>
            public async Task<byte[]> ToByteArrayAsync(CancellationToken cancellationToken = default)
            {
                ArgumentNullException.ThrowIfNull(stream);
                ResetPosition(stream);

                await using MemoryStream memoryStream = stream.CanSeek ? new MemoryStream((int)stream.Length) : new MemoryStream();
                await stream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
                return memoryStream.ToArray();
            }

            /// <summary>
            /// Reads the entire stream into a string asynchronously using the specified encoding.
            /// The stream position is reset to the beginning before reading.
            /// </summary>
            /// <param name="encoding">The character encoding to use. Defaults to <see cref="Encoding.UTF8"/> if <c>null</c>.</param>
            /// <param name="cancellationToken">An optional cancellation token.</param>
            /// <returns>A string containing the stream's contents.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the stream is <c>null</c>.</exception>
            public async Task<string> ToStringAsync(Encoding? encoding = null, CancellationToken cancellationToken = default)
            {
                ArgumentNullException.ThrowIfNull(stream);
                ResetPosition(stream);

                encoding ??= Encoding.UTF8;

                using var reader = new StreamReader(stream, encoding, leaveOpen: true);
                return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
            }

            /// <summary>
            /// Copies the entire stream into a new <see cref="MemoryStream"/>.
            /// The source stream position is reset to the beginning before copying.
            /// The returned <see cref="MemoryStream"/> is positioned at the beginning.
            /// </summary>
            /// <param name="cancellationToken">An optional cancellation token.</param>
            /// <returns>A new <see cref="MemoryStream"/> containing the source stream's contents.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the stream is <c>null</c>.</exception>
            public async Task<MemoryStream> ToMemoryStreamAsync(CancellationToken cancellationToken = default)
            {
                ArgumentNullException.ThrowIfNull(stream);
                ResetPosition(stream);

                MemoryStream memoryStream = stream.CanSeek ? new MemoryStream((int)stream.Length) : new MemoryStream();
                await stream.CopyToAsync(memoryStream, cancellationToken).ConfigureAwait(false);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }

        private static void ResetPosition(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
        }
    }
}

