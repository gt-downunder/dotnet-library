using System;
using System.Collections.Generic;

namespace DotNet.Library.Extensions
{
    public static class UriEx
    {
        public static IList<string> Properties(this Uri uri)
        {
            IList<string> properties =
            [
                $"AbsolutePath: {uri.AbsolutePath}",
                $"AbsoluteUri: {uri.AbsoluteUri}",
                $"Authority: {uri.Authority}",
                $"DnsSafeHost: {uri.DnsSafeHost}",
                $"Fragment: {uri.Fragment}",
                $"Host: {uri.Host}",
                "HostNameType: {0}".FillWith(uri.HostNameType.IsNullOrEmpty() ? "" : uri.HostNameType.ToString()),
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
            ];

            for (int i = 0; i < uri.Segments.Length; i++)
            {
                properties.Add($"Segment{i}: {uri.Segments[i]}");
            }

            properties.Add($"UserEscaped: {uri.UserEscaped}");
            properties.Add($"UserInfo: {uri.UserInfo}");

            return properties;
        }
    }
}
