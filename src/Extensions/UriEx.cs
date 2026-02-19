namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Uri"/> inspection and diagnostics.
    /// </summary>
    public static class UriEx
    {
        extension(Uri uri)
        {
            /// <summary>
            /// Returns a list of string representations of all key Uri properties.
            /// Intended for debugging/logging.
            /// </summary>
            /// <returns>A read-only list of formatted URI property strings.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the URI is <c>null</c>.</exception>
            public IReadOnlyList<string> DumpProperties()
            {
                ArgumentNullException.ThrowIfNull(uri);

                var properties = new List<string>
                {
                    $"AbsolutePath: {uri.AbsolutePath}",
                    $"AbsoluteUri: {uri.AbsoluteUri}",
                    $"Authority: {uri.Authority}",
                    $"DnsSafeHost: {uri.DnsSafeHost}",
                    $"Fragment: {uri.Fragment}",
                    $"Host: {uri.Host}",
                    $"HostNameType: {uri.HostNameType}",
                    $"IsAbsoluteUri: {uri.IsAbsoluteUri}",
                    $"IsDefaultPort: {uri.IsDefaultPort}",
                    $"IsFile: {uri.IsFile}",
                    $"IsLoopback: {uri.IsLoopback}",
                    $"IsUnc: {uri.IsUnc}",
                    $"LocalPath: {uri.LocalPath}",
                    $"OriginalString: {uri.OriginalString}",
                    $"PathAndQuery: {uri.PathAndQuery}",
                    $"Port: {uri.Port}",
                    $"Query: {uri.Query}",
                    $"Scheme: {uri.Scheme}"
                };

                properties.AddRange(uri.Segments.Select((seg, i) => $"Segment{i}: {seg}"));

                properties.Add($"UserEscaped: {uri.UserEscaped}");
                properties.Add($"UserInfo: {uri.UserInfo}");

                return properties;
            }
        }
    }
}
