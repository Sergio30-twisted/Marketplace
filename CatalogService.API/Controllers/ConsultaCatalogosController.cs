using Microsoft.AspNetCore.Mvc;
using Catologo.API.Application.Service;
using Catologo.API.Domain.Entities;
using Catologo.API.Application.Interface.Catalogo;

namespace CatalogService.API.Controllers;

[ApiController]
[Route("catalog")]
public class ConsultaCatalogosController : ControllerBase
{
    private readonly IConsultaDeCatalogoService _service;

    public ConsultaCatalogosController(IConsultaDeCatalogoService service)
    {
        _service = service;
    }

    [HttpGet("productos")]
    public async Task<IActionResult> GetProductos()
    {
        // Llamada asíncrona al servicio
        var productos = await _service.GetAll();
        return Ok(productos);
    }

    [HttpGet("productos/{id}")]
    public async Task<IActionResult> GetProducto(int id) // Cambiado a string
    {
        var producto = await _service.GetById(id);

        if (producto == null)
            return NotFound(new { message = $"Producto con ID {id} no encontrado" });

        return Ok(producto);
    }

    [HttpPost("productos")]
    public async Task<IActionResult> CrearProducto([FromBody] Producto product)
    {
        var nuevo = await _service.Add(product);
        // Es buena práctica devolver un 201 Created para nuevos recursos
        return CreatedAtAction(nameof(GetProducto), new { id = nuevo.Id }, nuevo);
    }

    [HttpPost("reservar")]
    public async Task<IActionResult> ReservarStock(int productId, int cantidad) // Cambiado a string
    {
        var result = await _service.UpdateStock(productId, cantidad);

        if (!result)
            return BadRequest(new { message = "Sin stock suficiente o producto no encontrado" });

        return Ok(new { message = "Stock reservado correctamente" });
    }
}