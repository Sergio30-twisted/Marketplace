using Auditoria_y_Trazabilidad.Application.Interface;
using Auditoria_y_Trazabilidad.Domain.Entities;
using Auditoria_y_Trazabilidad.Infrastructure.Interface;

namespace Auditoria_y_Trazabilidad.Application.Service
{
    public class ConsultorDeTrazabilidadService : IConsultorDeTrazabilidadService
    {
        private readonly IAuditoriaRepository _repository;
        private readonly ILogger<ConsultorDeTrazabilidadService> _logger;

        public ConsultorDeTrazabilidadService(
            IAuditoriaRepository repository,
            ILogger<ConsultorDeTrazabilidadService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<RegistroAuditoria>> RastrearFlujoPorCorrelationIdAsync(string correlationId)
        {
            _logger.LogInformation("Trazando flujo para CorrelationId: {CorrelationId}", correlationId);
            var registros = await _repository.ObtenerPorCorrelationIdAsync(correlationId);
            // Ordenar cronológicamente para visualizar el flujo completo
            return registros.OrderBy(r => r.FechaCreacion);
        }

        public async Task<RegistroAuditoria?> ObtenerRegistroPorIdAsync(string id)
        {
            _logger.LogInformation("Buscando registro con Id: {Id}", id);
            return await _repository.ObtenerPorIdAsync(id);
        }
    }
}
