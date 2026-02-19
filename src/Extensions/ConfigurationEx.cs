using Microsoft.Extensions.Configuration;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IConfiguration"/>.
    /// </summary>
    public static class ConfigurationEx
    {
        extension(IConfiguration configuration)
        {
            /// <summary>
            /// Gets the value of the specified configuration key.
            /// Throws <see cref="InvalidOperationException"/> if the key is missing or the value is empty.
            /// </summary>
            /// <param name="key">The configuration key.</param>
            /// <returns>The non-empty configuration value.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the configuration is <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException">Thrown if the key is missing or the value is null/empty.</exception>
            public string GetRequiredValue(string key)
            {
                ArgumentNullException.ThrowIfNull(configuration);
                ArgumentException.ThrowIfNullOrWhiteSpace(key);

                string? value = configuration[key];
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException($"Configuration key '{key}' is missing or empty.");
                }

                return value;
            }

            /// <summary>
            /// Gets the value of the specified configuration key, or returns a default value if the key is missing.
            /// </summary>
            /// <param name="key">The configuration key.</param>
            /// <param name="defaultValue">The default value to return if the key is missing or empty.</param>
            /// <returns>The configuration value, or <paramref name="defaultValue"/> if not found.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the configuration is <c>null</c>.</exception>
            public string GetValueOrDefault(string key, string defaultValue = "")
            {
                ArgumentNullException.ThrowIfNull(configuration);
                ArgumentException.ThrowIfNullOrWhiteSpace(key);

                string? value = configuration[key];
                return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
            }

            /// <summary>
            /// Determines whether the specified configuration key exists and has a non-empty value.
            /// </summary>
            /// <param name="key">The configuration key to check.</param>
            /// <returns><c>true</c> if the key exists and has a non-empty value; otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the configuration is <c>null</c>.</exception>
            public bool HasKey(string key)
            {
                ArgumentNullException.ThrowIfNull(configuration);
                ArgumentException.ThrowIfNullOrWhiteSpace(key);

                return !string.IsNullOrWhiteSpace(configuration[key]);
            }
        }
    }
}

