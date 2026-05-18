using Auditoria_y_Trazabilidad.Application.Interface;
using Auditoria_y_Trazabilidad.Domain.Entities;
using Auditoria_y_Trazabilidad.Infrastructure.Interface;

namespace Auditoria_y_Trazabilidad.Application.Service
{
    public class ReceptorDeLogsService : IReceptorDeLogsService
    {
        private readonly IAuditoriaRepository _repository;
        private readonly IEnriquecedorDeContextoService _enriquecedor;
        private readonly IVerificadorDeIntegridadService _verificador;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ReceptorDeLogsService> _logger;

        public ReceptorDeLogsService(
            IAuditoriaRepository repository,
            IEnriquecedorDeContextoService enriquecedor,
            IVerificadorDeIntegridadService verificador,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ReceptorDeLogsService> logger)
        {
            _repository = repository;
            _enriquecedor = enriquecedor;
            _verificador = verificador;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task RecibirLogAsync(RegistroAuditoria registro)
        {
            // 1. Enriquecer con datos del contexto HTTP
            if (_httpContextAccessor.HttpContext != null)
                registro = _enriquecedor.Enriquecer(registro, _httpContextAccessor.HttpContext);

            // 2. Generar hash de integridad
            registro.HashIntegridad = _verificador.GenerarHash(registro);

            // 3. Persistir en MongoDB
            await _repository.InsertarRegistroAsync(registro);

            _logger.LogInformation("Log recibido: [{Nivel}] {Servicio} - {Accion} | CID={CorrelationId}",
                registro.Nivel, registro.Servicio, registro.Accion, registro.CorrelationId);
        }
    }
}
