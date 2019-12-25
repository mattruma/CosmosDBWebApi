using CosmosDBWebApi.Helpers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CosmosDBWebApi.Data
{
    public class OrderCosmosDbSdk3Repository : CosmosDbSdk3Repository, IOrderCosmosDbSdk3Repository
    {
        public OrderCosmosDbSdk3Repository(
            IOptions<AzureCosmosDbOptions> azureCosmosDbOptions) : base(azureCosmosDbOptions)
        {
        }

        public async Task<Order> AddAsync(
            Order order)
        {
            var orderContainer =
               _cosmosDatabase.GetContainer("orders");

            var orderDocument =
                await orderContainer.CreateItemAsync<Order>(
                    order, 
                    new PartitionKey(order.Id.ToString()));

            return orderDocument.Resource;
        }

        public async Task<Order> DeleteByIdAsync(
            Guid id)
        {
            var orderContainer =
               _cosmosDatabase.GetContainer("orders");

            var orderDocument =
                await orderContainer.DeleteItemAsync<Order>(
                    id.ToString(),
                    new PartitionKey(id.ToString()));

            return orderDocument.Resource;
        }

        public async Task<Order> FetchByIdAsync(
            Guid id)
        {
            var orderContainer =
               _cosmosDatabase.GetContainer("orders");

            var orderDocument =
                await orderContainer.ReadItemAsync<Order>(
                    id.ToString(),
                    new PartitionKey(id.ToString()));

            return orderDocument.Resource;
        }

        public async Task<IEnumerable<Order>> FetchListAsync(
            Guid? itemId)
        {
            var orderContainer =
                _cosmosDatabase.GetContainer("orders");

            var query =
                $"SELECT * FROM o";

            if (itemId.HasValue)
            {
                query += $" WHERE ARRAY_CONTAINS(o.items, {{ \"id\": \"{itemId}\" }}, true)";
            }

            var queryDefinition =
                new QueryDefinition(query);

            var orders =
                orderContainer.GetItemQueryIterator<Order>(queryDefinition, null, new QueryRequestOptions() { MaxConcurrency = 2 });

            var orderList = new List<Order>();

            while (orders.HasMoreResults)
            {
                orderList.AddRange(
                    await orders.ReadNextAsync());
            };

            return orderList;
        }

        public async Task<Order> UpdateByIdAsync(
            Guid id,
            Order order)
        {
            var orderContainer =
               _cosmosDatabase.GetContainer("orders");

            var orderDocument =
                await orderContainer.ReplaceItemAsync<Order>(
                    order,
                    id.ToString(),
                    new PartitionKey(id.ToString())
                    );

            return orderDocument.Resource;
        }
    }
}
