using CosmosDBWebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDBWebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/orders")]
    [Produces("application/json")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderCosmosDbSdk2Repository _orderRepository;

        public OrdersController(
            IOrderCosmosDbSdk2Repository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost(Name = "AddOrder")]
        [ProducesResponseType(201, Type = typeof(Order))]
        [ProducesResponseType(400, Type = typeof(ModelStateDictionary))]
        [ProducesResponseType(401)]
        [SwaggerOperation(Tags = new[] { "Orders" })]
        public async Task<IActionResult> AddAsync(
            [FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result =
                await _orderRepository.AddAsync(
                    order);

            return CreatedAtRoute("FetchOrder", new { id = result.Id }, result);
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [SwaggerOperation(Tags = new[] { "Orders" })]
        public async Task<IActionResult> DeleteByIdAsync(
           [FromRoute] Guid id)
        {
            if (id == null
                || id == Guid.Empty)
            {
                return BadRequest("Id required.");
            }

            await _orderRepository.DeleteByIdAsync(
                id);

            return NoContent();
        }

        [HttpGet("{id}", Name = "FetchOrder")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(401)]
        [SwaggerOperation(Tags = new[] { "Orders" })]
        public async Task<IActionResult> FetchByIdAsync(
            [FromRoute] Guid id)
        {
            if (id == null
                || id == Guid.Empty)
            {
                return BadRequest("Id required.");
            }

            var result =
                await _orderRepository.FetchByIdAsync(
                    id);

            return Ok(result);
        }

        [HttpGet(Name = "FetchOrders")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        [ProducesResponseType(401)]
        [SwaggerOperation(Tags = new[] { "Orders" })]
        public async Task<IActionResult> FetchListAsync(
            [FromQuery]Guid? itemId)
        {
            var result =
                await _orderRepository.FetchListAsync(
                    itemId);

            return Ok(result);
        }

        [HttpPut("{id}", Name = "UpdateOrder")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(400, Type = typeof(ModelStateDictionary))]
        [ProducesResponseType(401)]
        [SwaggerOperation(Tags = new[] { "Orders" })]
        public async Task<IActionResult> UpdateByIdAsync(
            [FromRoute] Guid id,
            [FromBody] Order order)
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

            order.Id = id;

            var result =
                await _orderRepository.UpdateByIdAsync(
                    id, 
                    order);

            return Ok(result);
        }
    }
}
