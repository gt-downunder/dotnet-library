using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DotNet.Library.Extensions;
using DotNet.Library.Services.Abstractions;
using DotNet.Library.Services.Azure.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DotNet.Library.Services.Azure
{
    public class KeyVaultService : IAzureKeyVaultService
    {
        private readonly KeyVaultServiceOptions _options;
        private readonly ILogger<KeyVaultService> _logger;
        private readonly IMemoryCache _memoryCache;

        public KeyVaultService(ILogger<KeyVaultService> logger, IOptions<KeyVaultServiceOptions> options)
        {
            _logger = logger;
            _options = options.Value;
            _memoryCache = null;
        }

        public KeyVaultService(ILogger<KeyVaultService> logger, IOptions<KeyVaultServiceOptions> options, IMemoryCache memoryCache)
        {
            _logger = logger;
            _options = options.Value;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Gets an access token for the client ID and client key specified in the configuration.
        /// </summary>
        internal async Task<string> GetAccessToken(string[] scopes)
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(_options.ClientId)
                .WithClientSecret(_options.ClientKey)
                .Build();

            // TODO: Needs work - https://learn.microsoft.com/en-us/entra/msal/dotnet/acquiring-tokens/web-apps-apis/client-credential-flows#supported-client-credentials

            if (scopes.Length == 0)
            {
                scopes = ["some_app_id_uri/.default"];
            }

            AuthenticationResult result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;
        }

        /// <summary>
        /// Get an instance of the Azure Key Vault client
        /// </summary>
        internal SecretClient GetSecretClient(string vaultUrl)
        {
            const string tenantId = "tenant-id-goes-here-or-added-via-the-constructor";

            if (_options.ClientKey.IsNotNullOrEmpty())
            {
                // Use the defined ClientId/ClientKey for access to the vault
                _logger.LogDebug("{name} using ClientId/ClientKey for vault access.", nameof(KeyVaultService));
                return new SecretClient(new Uri(vaultUrl), new ClientSecretCredential(tenantId, _options.ClientId, _options.ClientKey));
            }
            else if (_options.CertificateThumbprint.IsNotNullOrEmpty())
            {
                // Use the defined ClientId/Certificate (using Thumbprint) for access to the vault
                _logger.LogDebug("{name} using ClientId/Certificate for vault access.", nameof(KeyVaultService));
                return new SecretClient(new Uri(vaultUrl), new ClientCertificateCredential(tenantId, _options.ClientId, FindCertificateByThumbprint(_options.CertificateThumbprint))); // todo: test this still works
            }
            else
            {
                // Use MSI credentials for access to the vault (when running in App Service or locally in VS)
                _logger.LogDebug("{name} using MSI for vault access.", nameof(KeyVaultService));
                return new SecretClient(new Uri(vaultUrl), new DefaultAzureCredential());
            }
        }

        /// <summary>
        /// Lookup installed certificate by thumbprint
        /// </summary>
        /// <param name="findValue"></param>
        internal static X509Certificate2 FindCertificateByThumbprint(string findValue)
        {
            // Search local machine certificate store
            X509Store store = new(StoreName.Root, StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                // Don't validate certs, since the test root isn't installed.
                X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindByThumbprint, findValue, false);

                return (col == null || col.Count == 0) ? null : col[0];
            }
            finally
            {
                store.Close();
            }
        }

        public bool IsValidSecretUrl(string secretUrl)
        {
            return secretUrl.StartsWith("https://")
                && secretUrl.ContainsIgnoreCase("vault.azure.net")
                && secretUrl.ContainsIgnoreCase("/secrets/")
                && !secretUrl.Split('/')[4].IsNullOrEmpty()
                && secretUrl.Count(x => x == '/') >= 4
                && secretUrl.Count(x => x == '/') <= 5;
        }

        public async Task<string> GetSecretAsync(string secretUrl)
        {
            if (!IsValidSecretUrl(secretUrl))
            {
                throw new ArgumentException($"The secret URL provided ('{secretUrl}') is invalid");
            }

            string baseUrl = secretUrl[..secretUrl.IndexOf("/secrets/", StringComparison.OrdinalIgnoreCase)];
            string secretName = secretUrl.SplitIgnoreCase($"{baseUrl}/secrets/")[1].Split('/')[0];
            string secretVersion = string.Empty;

            if (secretUrl.SplitIgnoreCase(secretName)[1]?.Length == 0 || secretUrl.SplitIgnoreCase(secretName)[1] == "/")
            {
                secretVersion = null;
            }
            else if (secretUrl.SplitIgnoreCase(secretName)[1].StartsWith('/'))
            {
                secretVersion = secretUrl.SplitIgnoreCase(secretName)[1].Split('/')[1];
            }

            return await GetSecretAsync(baseUrl, secretName, secretVersion);
        }

        public async Task<string> GetSecretAsync(string vaultBaseUrl, string secretName, string secretVersion = null)
        {
            try
            {
                if (_memoryCache != null && _options.CacheDuration > 0)
                {
                    return await _memoryCache.GetOrCreateAsync(GetSecretUrl(vaultBaseUrl, secretName, secretVersion), async entry =>
                    {
                        _logger.LogDebug("Refreshing cache entry for {url}, will expire after {cacheDuration} minutes.", GetSecretUrl(vaultBaseUrl, secretName, secretVersion), _options.CacheDuration);

                        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.CacheDuration);

                        SecretClient client = GetSecretClient(vaultBaseUrl);
                        KeyVaultSecret secretVal = (await client.GetSecretAsync(secretName, secretVersion)).Value;

                        return secretVal.Value;
                    });
                }
                else
                {
                    var client = GetSecretClient(vaultBaseUrl);
                    var secret = (await client.GetSecretAsync(secretName, secretVersion)).Value;

                    return secret.Value;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed attempting to access key vault secret '{url}'", GetSecretUrl(vaultBaseUrl, secretName, secretVersion));
                throw;
            }
        }

        private static string GetSecretUrl(string vaultBaseUrl, string secretName, string secretVersion) => $"{vaultBaseUrl}/{secretName}/{secretVersion}";
    }
}
