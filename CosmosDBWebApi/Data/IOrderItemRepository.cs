using CosmosDBWebApi.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDBWebApi.Data
{
    public interface IOrderItemRepository : ICosmosDbRepository
    {
        Task<OrderItem> AddAsync(
            Guid orderId,
            OrderItem orderItem);

        Task<OrderItem> DeleteByIdAsync(
            Guid orderId,
            Guid id);

        Task<OrderItem> FetchByIdAsync(
            Guid orderId,
            Guid id);

        Task<IEnumerable<OrderItem>> FetchListAsync(
            Guid orderId);

        Task<OrderItem> UpdateByIdAsync(
            Guid orderId,
            Guid id,
            OrderItem orderItem);
    }
}
