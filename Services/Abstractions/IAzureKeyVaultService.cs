using System.Threading.Tasks;

namespace DotNet.Library.Services.Abstractions
{
    public interface IAzureKeyVaultService
    {
        Task<string> GetSecretAsync(string vaultBaseUrl, string secretName, string secretVersion = null);
        Task<string> GetSecretAsync(string secretUrl);
        bool IsValidSecretUrl(string secretUrl);
    }
}
