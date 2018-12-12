using CosmosDBWebApi.Helpers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDBWebApi.Data
{
    public class OrderItemCosmosDbSdk3Repository : CosmosDbSdk3Repository, IOrderItemCosmosDbSdk3Repository
    {
        public OrderItemCosmosDbSdk3Repository(
            IOptions<AzureCosmosDbOptions> azureCosmosDbOptions) : base(azureCosmosDbOptions)
        {
        }

        public async Task<OrderItem> AddAsync(
            Guid orderId,
            OrderItem orderItem)
        {
            var orderContainer =
               _cosmosDatabase.Containers["orders"];

            var orderDocument =
                await orderContainer.Items.ReadItemAsync<Order>(
                    orderId.ToString(),
                    orderId.ToString());

            var order =
                orderDocument.Resource;

            order.Items.Add(
                orderItem);

            orderDocument =
                await orderContainer.Items.ReplaceItemAsync<Order>(
                    orderId.ToString(),
                    orderId.ToString(),
                    order);

            return orderItem;
        }

        public async Task<OrderItem> DeleteByIdAsync(
            Guid orderId,
            Guid id)
        {
            var orderContainer =
               _cosmosDatabase.Containers["orders"];

            var orderDocument =
                await orderContainer.Items.ReadItemAsync<Order>(
                    orderId.ToString(),
                    orderId.ToString());

            var order =
                orderDocument.Resource;

            var orderItem =
                order.Items.Single(x => x.Id == id);

            order.Items.Remove(
                orderItem);

            orderDocument =
                await orderContainer.Items.ReplaceItemAsync<Order>(
                    orderId.ToString(),
                    orderId.ToString(),
                    order);

            return orderItem;
        }

        public async Task<OrderItem> FetchByIdAsync(
            Guid orderId,
            Guid id)
        {
            var orderContainer =
               _cosmosDatabase.Containers["orders"];

            var orderDocument =
                await orderContainer.Items.ReadItemAsync<Order>(
                    orderId.ToString(),
                    orderId.ToString());

            var order =
                orderDocument.Resource;

            var orderItem =
                order.Items.Single(x => x.Id == id);

            return orderItem;
        }

        public async Task<IEnumerable<OrderItem>> FetchListAsync(
            Guid orderId)
        {
            var orderContainer =
                _cosmosDatabase.Containers["orders"];

            var query =
                $"SELECT i.id, i.name, i.quantity, i.isTaxable FROM o JOIN i IN o.items";

            //var query =
            //    $"SELECT i.id, i.name, i.quantity, i.isTaxable FROM o JOIN i IN o.items WHERE o.id = \"{orderId}\"";

            var queryDefinition =
                new CosmosSqlQueryDefinition(query);

            var orderItems =
                orderContainer.Items.CreateItemQuery<OrderItem>(queryDefinition, orderId.ToString());

            var orderItemList = new List<OrderItem>();

            while (orderItems.HasMoreResults)
            {
                orderItemList.AddRange(
                    await orderItems.FetchNextSetAsync());
            };

            return orderItemList;
        }

        public async Task<OrderItem> UpdateByIdAsync(
            Guid orderId,
            Guid id,
            OrderItem orderItem)
        {
            var orderContainer =
               _cosmosDatabase.Containers["orders"];

            var orderDocument =
                await orderContainer.Items.ReadItemAsync<Order>(
                    orderId.ToString(),
                    orderId.ToString());

            var order =
                orderDocument.Resource;

            var orderItemToBeUpdated =
                order.Items.Single(x => x.Id == id);

            orderItemToBeUpdated.Name = orderItem.Name;
            orderItemToBeUpdated.Quantity = orderItem.Quantity;
            orderItemToBeUpdated.IsTaxable = orderItem.IsTaxable;

            orderDocument =
                await orderContainer.Items.ReplaceItemAsync<Order>(
                    orderId.ToString(),
                    orderId.ToString(),
                    order);

            return orderItem;
        }
    }
}
