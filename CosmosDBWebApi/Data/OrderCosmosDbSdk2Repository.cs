using CosmosDBWebApi.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDBWebApi.Data
{
    public class OrderCosmosDbSdk2Repository : CosmosDbSdk2Repository, IOrderRepository
    {
        public OrderCosmosDbSdk2Repository(
          IOptions<AzureCosmosDbOptions> azureCosmosDbOptions) : base(azureCosmosDbOptions)
        {
        }

        public Task<Order> AddAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<Order> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> FetchByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> FetchList()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> FetchListByItemIdAsync(Guid itemId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateByIdAsync(Guid id, Order order)
        {
            throw new NotImplementedException();
        }
    }
}
