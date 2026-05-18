using Auditoria_y_Trazabilidad.Application.Interface;
using Auditoria_y_Trazabilidad.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Auditoria_y_Trazabilidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceptorDeLogsController : ControllerBase
    {
        private readonly IReceptorDeLogsService _service;

        public ReceptorDeLogsController(IReceptorDeLogsService service)
        {
            _service = service;
        }

        /// Recibe y persiste un evento de auditoría desde cualquier microservicio.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RecibirLog([FromBody] RegistroAuditoria registro)
        {
            if (string.IsNullOrWhiteSpace(registro.Servicio) || string.IsNullOrWhiteSpace(registro.Accion))
                return BadRequest(new { error = "Los campos 'Servicio' y 'Accion' son obligatorios." });

            await _service.RecibirLogAsync(registro);
            return StatusCode(201, new { mensaje = "Log registrado correctamente.", correlationId = registro.CorrelationId });
        }
    }
}
