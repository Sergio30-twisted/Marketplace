using Auditoria_y_Trazabilidad.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Auditoria_y_Trazabilidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultorDeTrazabilidadController : ControllerBase
    {
        private readonly IConsultorDeTrazabilidadService _service;

        public ConsultorDeTrazabilidadController(IConsultorDeTrazabilidadService service)
        {
            _service = service;
        }

        ///Retorna todos los eventos de un flujo distribuido ordenados cronológicamente.
        [HttpGet("flujo/{correlationId}")]
        public async Task<IActionResult> RastrearFlujo(string correlationId)
        {
            var flujo = await _service.RastrearFlujoPorCorrelationIdAsync(correlationId);
            return Ok(flujo);
        }

        /// Obtiene un registro específico por su Id de MongoDB.
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(string id)
        {
            var registro = await _service.ObtenerRegistroPorIdAsync(id);
            if (registro is null)
                return NotFound(new { error = $"Registro con Id '{id}' no encontrado." });

            return Ok(registro);
        }
    }
}
