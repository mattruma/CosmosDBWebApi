using CosmosDBWebApi.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDBWebApi.Data
{
    public interface IOrderRepository : ICosmosDbRepository
    {
        Task<Order> AddAsync(
            Order order);

        Task<Order> DeleteByIdAsync(
            Guid id);

        Task<Order> FetchByIdAsync(
            Guid id);

        Task<IEnumerable<Order>> FetchListAsync(
            Guid? itemId);

        Task<Order> UpdateByIdAsync(
            Guid id,
            Order order);
    }
}
