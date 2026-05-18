using Auditoria_y_Trazabilidad.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Auditoria_y_Trazabilidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GestorDeAuditoriaController : ControllerBase
    {
        private readonly IGestorDeAuditoriaService _service;

        public GestorDeAuditoriaController(IGestorDeAuditoriaService service)
        {
            _service = service;
        }

        /// Obtiene los últimos 500 registros de auditoría.
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var registros = await _service.ObtenerTodosAsync();
            return Ok(registros);
        }

        ///Filtra registros por nombre de microservicio.
        [HttpGet("servicio/{servicio}")]
        public async Task<IActionResult> ObtenerPorServicio(string servicio)
        {
            var registros = await _service.ObtenerPorServicioAsync(servicio);
            return Ok(registros);
        }

        /// Filtra registros por rango de fechas (UTC).
        [HttpGet("fechas")]
        public async Task<IActionResult> ObtenerPorFechas(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta)
        {
            if (desde >= hasta)
                return BadRequest(new { error = "'desde' debe ser anterior a 'hasta'." });

            var registros = await _service.ObtenerPorFechasAsync(desde, hasta);
            return Ok(registros);
        }
    }
}
