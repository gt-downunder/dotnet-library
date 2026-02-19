using System.Security.Cryptography;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for byte arrays, including encoding and hashing operations.
    /// </summary>
    public static class ByteArrayEx
    {
        extension(byte[] data)
        {
            /// <summary>
            /// Converts the byte array to a lowercase hexadecimal string.
            /// </summary>
            /// <returns>A lowercase hex string representation of the byte array.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the byte array is null.</exception>
            public string ToHexString()
            {
                ArgumentNullException.ThrowIfNull(data);
                return Convert.ToHexString(data).ToLowerInvariant();
            }

            /// <summary>
            /// Converts the byte array to a Base64-encoded string.
            /// </summary>
            /// <returns>A Base64-encoded string.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the byte array is null.</exception>
            public string ToBase64()
            {
                ArgumentNullException.ThrowIfNull(data);
                return Convert.ToBase64String(data);
            }

            /// <summary>
            /// Computes the SHA-256 hash of the byte array.
            /// </summary>
            /// <returns>A 32-byte array containing the SHA-256 hash.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the byte array is null.</exception>
            public byte[] ComputeSha256()
            {
                ArgumentNullException.ThrowIfNull(data);
                return SHA256.HashData(data);
            }
        }

        extension(string hex)
        {
            /// <summary>
            /// Converts a hexadecimal string to a byte array.
            /// </summary>
            /// <returns>The byte array represented by the hex string.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
            /// <exception cref="FormatException">Thrown if the string is not a valid hex string.</exception>
            public byte[] FromHexString()
            {
                ArgumentNullException.ThrowIfNull(hex);
                return Convert.FromHexString(hex);
            }
        }

        extension(string base64)
        {
            /// <summary>
            /// Converts a Base64-encoded string to a byte array.
            /// </summary>
            /// <returns>The byte array represented by the Base64 string.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
            /// <exception cref="FormatException">Thrown if the string is not a valid Base64 string.</exception>
            public byte[] FromBase64ToBytes()
            {
                ArgumentNullException.ThrowIfNull(base64);
                return Convert.FromBase64String(base64);
            }
        }
    }
}

