namespace DotNet.Library.Services.Azure.Options
{
    public class KeyVaultServiceOptions
    {
        public string Authority { get; set; }

        public string ClientId { get; set; }

        // If this option is set use ClientID/ClientKey for vault access
        public string ClientKey { get; set; }

        // If this option is set use ClientID/Certificate for vault access
        public string CertificateThumbprint { get; set; }

        // Set the cache duration for a secret in minutes.  The default value is 0 which results in no cached values.
        public int CacheDuration { get; set; } = 0;
    }
}
