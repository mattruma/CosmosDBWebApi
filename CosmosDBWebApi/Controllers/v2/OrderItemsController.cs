using Microsoft.AspNetCore.Mvc;

namespace CosmosDBWebApi.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/orders/{orderId}/items")]
    [Produces("application/json")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
    }
}