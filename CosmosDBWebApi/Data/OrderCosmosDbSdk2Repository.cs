using CosmosDBWebApi.Helpers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDBWebApi.Data
{
    public class OrderCosmosDbSdk2Repository : CosmosDbSdk2Repository, IOrderCosmosDbSdk2Repository
    {
        public OrderCosmosDbSdk2Repository(
          IOptions<AzureCosmosDbOptions> azureCosmosDbOptions) : base(azureCosmosDbOptions)
        {
        }

        public async Task<Order> AddAsync(
            Order order)
        {
            var requestOptions =
                new RequestOptions
                {
                    PartitionKey = new PartitionKey(order.Id.ToString())
                };

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
