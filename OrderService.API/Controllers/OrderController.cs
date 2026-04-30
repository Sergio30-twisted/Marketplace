using Microsoft.AspNetCore.Mvc;
using OrderService.API.Models;
using OrderService.API.Services;
namespace OrderService.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly OrderProcessor _service;

    public OrderController(OrderProcessor service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CrearOrden(Order order)
    {
        var result = await _service.ProcesarOrden(order);
        return Ok(result);
    }
}