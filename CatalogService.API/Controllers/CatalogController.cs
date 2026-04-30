using Microsoft.AspNetCore.Mvc;
using CatalogService.API.Services;
using CatalogService.API.Models;

namespace CatalogService.API.Controllers;

[ApiController]
[Route("catalog")]
public class CatalogController : ControllerBase
{
    private readonly CatalogManager _service;

    public CatalogController(CatalogManager service)
    {
        _service = service;
    }

    [HttpGet("productos")]
    public IActionResult GetProductos()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("productos/{id}")]
    public IActionResult GetProducto(int id)
    {
        var producto = _service.GetById(id);

        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    [HttpPost("productos")]
    public IActionResult CrearProducto(Product product)
    {
        var nuevo = _service.Add(product);
        return Ok(nuevo);
    }

    [HttpPost("reservar")]
    public IActionResult ReservarStock(int productId, int cantidad)
    {
        var result = _service.UpdateStock(productId, cantidad);

        if (!result)
            return BadRequest("Sin stock");

        return Ok("Stock reservado");
    }
}