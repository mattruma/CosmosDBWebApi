using CosmosDBWebApi.Helpers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDBWebApi.Data
{
    public class OrderCosmosDbSdk3Repository : CosmosDbSdk3Repository, IOrderRepository
    {
        public OrderCosmosDbSdk3Repository(
            IOptions<AzureCosmosDbOptions> azureCosmosDbOptions) : base(azureCosmosDbOptions)
        {
        }

        public async Task<Order> AddAsync(
            Order order)
        {
            var orderContainer =
               _cosmosDatabase.Containers["orders"];

            var orderDocument =
                await orderContainer.Items.CreateItemAsync<Order>(
                    order.Id.ToString(), 
                    order);

            return orderDocument.Resource;
        }

        public async Task<Order> DeleteByIdAsync(
            Guid id)
        {
            var orderContainer =
               _cosmosDatabase.Containers["orders"];

            var orderDocument =
                await orderContainer.Items.DeleteItemAsync<Order>(
                    id.ToString(), 
                    id.ToString());

            return orderDocument.Resource;
        }

        public async Task<Order> FetchByIdAsync(
            Guid id)
        {
            var orderContainer =
               _cosmosDatabase.Containers["orders"];

            var orderDocument =
                await orderContainer.Items.ReadItemAsync<Order>(
                    id.ToString(), 
                    id.ToString());

            return orderDocument.Resource;
        }

        public Task<IEnumerable<Order>> FetchList()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> FetchListAsync(
            Guid? teamId)
        {
            var orderContainer =
                _cosmosDatabase.Containers["orders"];

            var query =
                $"SELECT * FROM l";

            if (teamId.HasValue)
            {
                query += $" WHERE ARRAY_CONTAINS(l.teams, \"{teamId}\", true)";
            }

            var queryDefinition =
                new CosmosSqlQueryDefinition(query);

            var orders =
                orderContainer.Items.CreateItemQuery<Order>(queryDefinition, maxConcurrency: 2);

            var orderList = new List<Order>();

            while (orders.HasMoreResults)
            {
                orderList.AddRange(
                    await orders.FetchNextSetAsync());
            };

            return orderList;
        }

        public Task<IEnumerable<Order>> FetchListByItemIdAsync(Guid itemId)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> UpdateByIdAsync(
            Guid id,
            Order order)
        {
            var orderContainer =
               _cosmosDatabase.Containers["orders"];

            var orderDocument =
                await orderContainer.Items.ReplaceItemAsync<Order>(
                    id.ToString(), 
                    id.ToString(), 
                    order);

            return orderDocument.Resource;
        }
    }
}
