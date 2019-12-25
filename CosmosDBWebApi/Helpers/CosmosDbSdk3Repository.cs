using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace CosmosDBWebApi.Helpers
{
    public abstract class CosmosDbSdk3Repository : ICosmosDbRepository
    {
        private readonly IOptions<AzureCosmosDbOptions> _azureCosmosDbOptions;
        private readonly CosmosClient _cosmosClient;
        protected readonly Database _cosmosDatabase;

        public CosmosDbSdk3Repository(
            IOptions<AzureCosmosDbOptions> azureCosmosDbOptions)
        {
            _azureCosmosDbOptions =
                azureCosmosDbOptions;

            _cosmosClient =
                new CosmosClient(
                    _azureCosmosDbOptions.Value.Endpoint,
                    _azureCosmosDbOptions.Value.Key);
            _cosmosDatabase = _cosmosClient.GetDatabase(_azureCosmosDbOptions.Value.DatabaseId);
        }
    }
}
