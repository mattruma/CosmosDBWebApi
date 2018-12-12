using CosmosDBWebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDBWebApi.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/orders/{orderId}/items")]
    [Produces("application/json")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemCosmosDbSdk3Repository _orderItemRepository;

        public OrderItemsController(
            IOrderItemCosmosDbSdk3Repository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        [HttpPost(Name = "AddOrderItem")]
        [ProducesResponseType(201, Type = typeof(OrderItem))]
        [ProducesResponseType(400, Type = typeof(ModelStateDictionary))]
        [ProducesResponseType(401)]
        [SwaggerOperation(Tags = new[] { "Order Items" })]
        public async Task<IActionResult> AddAsync(
            [FromRoute] Guid orderId,
            [FromBody] OrderItem orderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result =
                await _orderItemRepository.AddAsync(
                    orderId,
                    orderItem);

            return CreatedAtRoute("FetchOrderItem", new { orderId = orderId, id = result.Id }, result);
        }

        [HttpDelete("{id}", Name = "DeleteOrderItem")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [SwaggerOperation(Tags = new[] { "Order Items" })]
        public async Task<IActionResult> DeleteByIdAsync(
            [FromRoute] Guid orderId,
            [FromRoute] Guid id)
        {
            if (id == null
                || id == Guid.Empty)
            {
                return BadRequest("Id required.");
            }

            await _orderItemRepository.DeleteByIdAsync(
                orderId,
                id);

            return NoContent();
        }

        [HttpGet("{id}", Name = "FetchOrderItem")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(401)]
        [SwaggerOperation(Tags = new[] { "Order Items" })]
        public async Task<IActionResult> FetchByIdAsync(
            [FromRoute] Guid orderId,
            [FromRoute] Guid id)
        {
            if (id == null
                || id == Guid.Empty)
            {
                return BadRequest("Id required.");
            }

            var result =
                await _orderItemRepository.FetchByIdAsync(
                    orderId,
                    id);

            return Ok(result);
        }

        [HttpGet(Name = "FetchOrderItems")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        [ProducesResponseType(401)]
        [SwaggerOperation(Tags = new[] { "Order Items" })]
        public async Task<IActionResult> FetchListAsync(
            [FromRoute] Guid orderId)
        {
            var result =
                await _orderItemRepository.FetchListAsync(
                    orderId);

            return Ok(result);
        }

        [HttpPut("{id}", Name = "UpdateOrderItem")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(400, Type = typeof(ModelStateDictionary))]
        [ProducesResponseType(401)]
        [SwaggerOperation(Tags = new[] { "Order Items" })]
        public async Task<IActionResult> UpdateByIdAsync(
            [FromRoute] Guid orderId,
            [FromRoute] Guid id,
            [FromBody] OrderItem orderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == null
                || id == Guid.Empty)
            {
                return BadRequest("Id required.");
            }

            orderItem.Id = id;

            var result =
                await _orderItemRepository.UpdateByIdAsync(
                    orderId, 
                    id, 
                    orderItem);

            return Ok(result);
        }
    }
}