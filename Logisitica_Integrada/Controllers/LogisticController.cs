using Logisitica_Integrada.Models;
using Logisitica_Integrada.Services;
using Microsoft.AspNetCore.Mvc;

namespace Logisitica_Integrada.Controllers
{
    // 🛠️ ADAPTACIÓN OBLIGATORIA: Indicamos que es una API y forzamos la ruta exacta que busca NGINX
    [ApiController]
    [Route("api/Logistic")]
    public class LogisticsController : ControllerBase
    {
        private readonly ShipmentManager _manager;
        private readonly ShipmentRepository _repository;

        public LogisticsController(ShipmentManager manager, ShipmentRepository repository)
        {
            _manager = manager;
            _repository = repository;
        }

        // =========================================================================
        // 🛠️ AÑADIMOS UN MÉTODO RAÍZ EN EL CONTROLADOR PARA EVITAR EL 404 EN LA URL DIRECTA
        // =========================================================================
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(new
            {
                mensaje = "¡Servicio de Logística e Historial funcionando en Docker!",
                estado = "Conectado a NGINX"
            });
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
}