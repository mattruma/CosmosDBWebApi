using CosmosDBWebApi.Helpers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDBWebApi.Data
{
    public class OrderCosmosDbSdk2Repository : CosmosDbSdk2Repository, IOrderCosmosDbSdk2Repository
    {
        public OrderCosmosDbSdk2Repository(
          IOptions<AzureCosmosDbOptions> azureCosmosDbOptions) : base(azureCosmosDbOptions)
        {
        }

        // https://github.com/Azure/azure-cosmos-dotnet-v2/blob/f374cc601f4cf08d11c88f0c3fa7dcefaf7ecfe8/samples/code-samples/DocumentManagement/Program.cs#L198

        public async Task<Order> AddAsync(
            Order order)
        {
            var requestOptions =
                new RequestOptions
                {
                    PartitionKey = new PartitionKey(order.Id.ToString())
                };

            // Remember, to do bulk insert of documents it is recommended to use a Stored Procedure
            // and pass a batch of documents to the Stored Prcoedure. This will cut down on the number
            // of roundtrips required. 

            var orderDocument = await _documentClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(
                    _azureCosmosDbOptions.Value.DatabaseId, "orders"), order, requestOptions);

            return
                (Order)((dynamic)orderDocument.Resource);
        }

        public async Task<Order> DeleteByIdAsync(
            Guid id)
        {
            var requestOptions =
               new RequestOptions
               {
                   PartitionKey = new PartitionKey(id.ToString())
               };

            var orderDocument = await _documentClient.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(
                    _azureCosmosDbOptions.Value.DatabaseId, "orders", id.ToString()), requestOptions);

            return
                (Order)((dynamic)orderDocument.Resource);
        }

        public async Task<Order> FetchByIdAsync(
            Guid id)
        {
            var requestOptions =
                new RequestOptions
                {
                    PartitionKey = new PartitionKey(id.ToString())
                };

            var orderDocument =
                await _documentClient.ReadDocumentAsync(
                    UriFactory.CreateDocumentUri(
                        _azureCosmosDbOptions.Value.DatabaseId, "orders", id.ToString()), requestOptions);

            return
                (Order)((dynamic)orderDocument.Resource);
        }

        public async Task<IEnumerable<Order>> FetchListAsync(
            Guid? itemId)
        {
            var feedOptions =
                  new FeedOptions
                  {
                      MaxItemCount = -1,
                      EnableCrossPartitionQuery = true
                  };

            var query =
                $"SELECT * FROM o";

            if (itemId.HasValue)
            {
                query += $" WHERE ARRAY_CONTAINS(o.items, {{ \"id\": \"{itemId}\" }}, true)";
            }

            var queryDefinition =
                new SqlQuerySpec(query);

            // Can do some LINQ expressions, but limited support, e.g. does not support Any, Contains, etc.

            // var querySalesOrder = _documentClient.CreateDocumentQuery<Order>(
            //     UriFactory.CreateDocumentCollectionUri(
            //         _azureCosmosDbOptions.Value.DatabaseId, "orders"))
            //             .Where(o => o.Id == new Guid("b247199e-0483-4c12-a40a-66dfc15a1d25"))
            //             .AsEnumerable();

            var orderDocumentQuery =
                _documentClient.CreateDocumentQuery<Order>(
                    UriFactory.CreateDocumentCollectionUri(
                        _azureCosmosDbOptions.Value.DatabaseId, "orders"), queryDefinition, feedOptions)
                    .AsDocumentQuery();

            var orderList =
                new List<Order>();

            while (orderDocumentQuery.HasMoreResults)
            {
                orderList.AddRange(
                    await orderDocumentQuery.ExecuteNextAsync<Order>());
            }

            return orderList;
        }

        public async Task<Order> UpdateByIdAsync(
            Guid id,
            Order order)
        {
            var requestOptions =
               new RequestOptions
               {
                   PartitionKey = new PartitionKey(id.ToString())
               };

            var orderDocument = await _documentClient.ReplaceDocumentAsync(
                 UriFactory.CreateDocumentUri(
                     _azureCosmosDbOptions.Value.DatabaseId, "orders", id.ToString()), order, requestOptions);

            return
                (Order)((dynamic)orderDocument.Resource);
        }
    }
}
