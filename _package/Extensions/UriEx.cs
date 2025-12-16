namespace DotNet.Library.Extensions
{
    public static class UriEx
    {
        /// <summary>
        /// Returns a list of string representations of all key Uri properties.
        /// Intended for debugging/logging.
        /// </summary>
        public static IReadOnlyList<string> DumpProperties(this Uri uri)
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