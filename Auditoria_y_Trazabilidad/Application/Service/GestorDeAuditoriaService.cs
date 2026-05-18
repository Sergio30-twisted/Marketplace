using Auditoria_y_Trazabilidad.Application.Interface;
using Auditoria_y_Trazabilidad.Domain.Entities;
using Auditoria_y_Trazabilidad.Infrastructure.Interface;

namespace Auditoria_y_Trazabilidad.Application.Service
{
    public class GestorDeAuditoriaService : IGestorDeAuditoriaService
    {
        private readonly IAuditoriaRepository _repository;
        private readonly ILogger<GestorDeAuditoriaService> _logger;

        public GestorDeAuditoriaService(
            IAuditoriaRepository repository,
            ILogger<GestorDeAuditoriaService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<RegistroAuditoria>> ObtenerTodosAsync()
        {
            _logger.LogInformation("Consultando todos los registros de auditoría");
            return await _repository.ObtenerTodosAsync();
        }

        public async Task<IEnumerable<RegistroAuditoria>> ObtenerPorServicioAsync(string servicio)
        {
            _logger.LogInformation("Consultando registros del servicio: {Servicio}", servicio);
            return await _repository.ObtenerPorServicioAsync(servicio);
        }

        public async Task<IEnumerable<RegistroAuditoria>> ObtenerPorFechasAsync(DateTime desde, DateTime hasta)
        {
            _logger.LogInformation("Consultando registros entre {Desde} y {Hasta}", desde, hasta);
            return await _repository.ObtenerPorFechasAsync(desde, hasta);
        }
    }
}
