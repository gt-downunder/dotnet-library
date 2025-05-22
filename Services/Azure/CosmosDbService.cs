using DotNet.Library.Extensions;
using DotNet.Library.Services.Abstractions;
using DotNet.Library.Services.Azure.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Library.Services.Azure
{
    public class CosmosDbService(ILogger<CosmosDbService> logger, IOptions<CosmosDbServiceOptions> options, IAzureKeyVaultService vault) : IAzureCosmosDbService
    {
        private readonly ILogger<CosmosDbService> _logger = logger;
        private readonly CosmosDbServiceOptions _options = options.Value;
        private readonly IAzureKeyVaultService _vault = vault;
        private string _authKey;
        private CosmosClient _client;

        /// <summary>
        /// Gets the document specified by the provided documentId
        /// </summary>
        public async Task<T> GetDocumentAsync<T>(string documentId, string partitionKey = null, CancellationToken cancellationToken = default)
        {
            Container container = await GetClientContainerAsync();
            string key = partitionKey.IsNullOrEmpty() ? documentId : partitionKey;

            _logger.LogInformation("Retrieving document with id '{DocumentId}' and partition key '{PartitionKey}'", documentId, key);

            return await container.ReadItemAsync<T>(id: documentId, partitionKey: new PartitionKey(key), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Replaces the entire Document with the new document provided
        /// </summary>
        public async Task<T> ReplaceDocumentAsync<T>(T document, string id, string partitionKey = null, CancellationToken cancellationToken = default)
        {
            Container container = await GetClientContainerAsync();
            string key = partitionKey.IsNullOrEmpty() ? id : partitionKey;

            _logger.LogInformation("Replacing document with id '{DocumentId}' and partition key '{PartitionKey}'", id, key);

            return await container.ReplaceItemAsync<T>(item: document, id, partitionKey: new PartitionKey(key), cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Get all documents in the specified database and container
        /// </summary>
        public async Task<IEnumerable<T>> GetAllDocumentsAsync<T>(CancellationToken cancellationToken = default)
        {
            Container container = await GetClientContainerAsync();
            using FeedIterator<T> linqFeed = container.GetItemLinqQueryable<T>().ToFeedIterator();

            var results = new List<T>();

            // Iterate query result pages
            while (linqFeed.HasMoreResults)
            {
                FeedResponse<T> response = await linqFeed.ReadNextAsync(cancellationToken: cancellationToken);

                // Iterate query results
                results.AddRange(response);
            }

            _logger.LogInformation("Retrieved {Count} documents", results.Count);

            return results;
        }

        private async Task<Container> GetClientContainerAsync()
        {
            if (_authKey.IsNullOrEmpty())
            {
                _authKey = await _vault.GetSecretAsync(_options.Key);
            }

            // Client is thread safe and is recommended to be re-used
            _client ??= new CosmosClient(accountEndpoint: _options.UriEndpoint, _authKey, new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            });

            _logger.LogInformation("Creating container '{Database}/{Container}'", _options.Database, _options.Container);

            return _client.GetContainer(_options.Database, _options.Container);
        }
    }
}