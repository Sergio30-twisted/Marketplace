using Auditoria_y_Trazabilidad.Application.Interface;
using Auditoria_y_Trazabilidad.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Auditoria_y_Trazabilidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnriquecedorDeContextoController : ControllerBase
    {
        private readonly IEnriquecedorDeContextoService _service;

        public EnriquecedorDeContextoController(IEnriquecedorDeContextoService service)
        {
            _service = service;
        }

        /// 
        /// Enriquece un registro de auditoría con datos del contexto HTTP actual
        /// (IP, CorrelationId, usuario) sin persistirlo. Útil para pre-visualizar
        /// qué datos se agregarán antes de enviar al ReceptorDeLogs.
        /// 
        [HttpPost("preview")]
        public IActionResult PreviewEnriquecimiento([FromBody] RegistroAuditoria registro)
        {
            var enriquecido = _service.Enriquecer(registro, HttpContext);
            return Ok(enriquecido);
        }
    }
}
