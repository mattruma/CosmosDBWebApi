using Microsoft.AspNetCore.Mvc;

namespace CosmosDBWebApi.Controllers
{
    [Route("api/orders/{orderId}/items")]
    [Produces("application/json")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
    }
}