using System.Text.Json;

namespace Grondo.Utilities
{
    /// <summary>
    /// Provides pre-configured, cached <see cref="JsonSerializerOptions"/> instances
    /// for consistent JSON serialization across the library.
    /// </summary>
    internal static class JsonDefaults
    {
        /// <summary>Default options with no special configuration.</summary>
        internal static readonly JsonSerializerOptions Default = new();

        /// <summary>Options with case-insensitive property name matching.</summary>
        internal static readonly JsonSerializerOptions CaseInsensitive = new() { PropertyNameCaseInsensitive = true };

        /// <summary>Options with indented (pretty-printed) output.</summary>
        internal static readonly JsonSerializerOptions Indented = new() { WriteIndented = true };
    }
}
