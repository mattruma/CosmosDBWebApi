using Microsoft.AspNetCore.Mvc;

namespace CosmosDBWebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/orders/{orderId}/items")]
    [Produces("application/json")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
    }
}