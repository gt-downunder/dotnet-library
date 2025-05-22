using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Library.Services.Abstractions
{
    public interface IAzureCosmosDbService
    {
        Task<T> GetDocumentAsync<T>(string documentId, string partitionKey = null, CancellationToken cancellationToken = default);

        Task<T> ReplaceDocumentAsync<T>(T document, string id, string partitionKey = null, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllDocumentsAsync<T>(CancellationToken cancellationToken = default);
    }
}
