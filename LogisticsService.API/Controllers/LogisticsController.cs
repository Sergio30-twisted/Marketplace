using Microsoft.AspNetCore.Mvc;
using LogisticsService.API.Models;
using LogisticsService.API.Services;

namespace LogisticsService.API.Controllers;

[ApiController]
[Route("api/logistica")]
public class LogisticsController : ControllerBase
{
    private readonly ShipmentManager _manager;
    private readonly ShipmentRepository _repository;

    public LogisticsController(ShipmentManager manager, ShipmentRepository repository)
    {
        _manager = manager;
        _repository = repository;
    }

    [HttpGet("guia/{numeroGuia}")]
    public IActionResult ConsultarGuia(string numeroGuia)
    {
        var shipment = _repository.GetByNumeroGuia(numeroGuia);
        if (shipment == null)
            return NotFound($"No se encontró ningún envío con la guía '{numeroGuia}'.");
        return Ok(shipment);
    }

    [HttpGet("orden/{orderId}")]
    public IActionResult ConsultarPorOrden(int orderId)
    {
        var shipment = _repository.GetByOrderId(orderId);
        if (shipment == null)
            return NotFound($"No se encontró envío para la orden {orderId}.");
        return Ok(shipment);
    }

    [HttpGet("envios")]
    public IActionResult ListarEnvios()
    {
        return Ok(_repository.GetAll());
    }

    [HttpPut("guia/{numeroGuia}/estado")]
    public IActionResult ActualizarEstado(string numeroGuia, [FromBody] ActualizarEstadoRequest request)
    {
        var resultado = _manager.ActualizarEstado(numeroGuia, request.NuevoEstado);
        if (!resultado)
            return BadRequest($"No se pudo actualizar. Verifique la guía '{numeroGuia}' y que el estado sea válido.");
        return Ok($"Estado de la guía '{numeroGuia}' actualizado a '{request.NuevoEstado}'.");
    }

    [HttpPost("envios")]
    public IActionResult CrearEnvioManual([FromBody] OrderCreatedEvent evento)
    {
        var shipment = _manager.CrearEnvio(evento);
        return Ok(shipment);
    }
}

public class ActualizarEstadoRequest
{
    public string NuevoEstado { get; set; } = string.Empty;
}