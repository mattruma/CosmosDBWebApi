using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace CosmosDBWebApi.Helpers
{
    public class CosmosDbSdk2Repository : ICosmosDbRepository
    {
        protected readonly IOptions<AzureCosmosDbOptions> _azureCosmosDbOptions;
        protected readonly IDocumentClient _documentClient;

        public CosmosDbSdk2Repository(
            IOptions<AzureCosmosDbOptions> azureCosmosDbOptions)
        {
            _azureCosmosDbOptions =
                azureCosmosDbOptions;

            _documentClient =
                new DocumentClient(
                    _azureCosmosDbOptions.Value.EndpointAsUri,
                    _azureCosmosDbOptions.Value.Key);
        }
    }
}
