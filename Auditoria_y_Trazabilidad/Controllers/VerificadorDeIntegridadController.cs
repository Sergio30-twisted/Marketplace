using Auditoria_y_Trazabilidad.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Auditoria_y_Trazabilidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificadorDeIntegridadController : ControllerBase
    {
        private readonly IVerificadorDeIntegridadService _service;

        public VerificadorDeIntegridadController(IVerificadorDeIntegridadService service)
        {
            _service = service;
        }

        /// Verifica si el hash de un registro coincide con su contenido actual.
        [HttpGet("{id}")]
        public async Task<IActionResult> VerificarRegistro(string id)
        {
            var integro = await _service.VerificarIntegridadAsync(id);
            return Ok(new
            {
                id,
                integro,
                mensaje = integro
                    ? "El registro no ha sido alterado."
                    : "⚠️ ALERTA: El registro presenta inconsistencias de integridad."
            });
        }

        /// Escanea todos los registros y lista los que tienen hash inválido.
        [HttpGet("escanear")]
        public async Task<IActionResult> EscanearIntegridad()
        {
            var corruptos = await _service.DetectarRegistrosCorruptosAsync();
            return Ok(new
            {
                totalCorruptos = corruptos.Count(),
                registros = corruptos
            });
        }
    }
}
