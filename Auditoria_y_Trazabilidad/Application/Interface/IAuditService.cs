using Auditoria_y_Trazabilidad.Domain.Entities;

namespace Auditoria_y_Trazabilidad.Application.Interface
{
    // ── Receptor de Logs ──────────────────────────────────────────────────────
    public interface IReceptorDeLogsService
    {
        Task RecibirLogAsync(RegistroAuditoria registro);
    }

    // ── Gestor de Auditoría ───────────────────────────────────────────────────
    public interface IGestorDeAuditoriaService
    {
        Task<IEnumerable<RegistroAuditoria>> ObtenerTodosAsync();
        Task<IEnumerable<RegistroAuditoria>> ObtenerPorServicioAsync(string servicio);
        Task<IEnumerable<RegistroAuditoria>> ObtenerPorFechasAsync(DateTime desde, DateTime hasta);
    }

    // ── Consultor de Trazabilidad ─────────────────────────────────────────────
    public interface IConsultorDeTrazabilidadService
    {
        Task<IEnumerable<RegistroAuditoria>> RastrearFlujoPorCorrelationIdAsync(string correlationId);
        Task<RegistroAuditoria?> ObtenerRegistroPorIdAsync(string id);
    }

    // ── Enriquecedor de Contexto ──────────────────────────────────────────────
    public interface IEnriquecedorDeContextoService
    {
        RegistroAuditoria Enriquecer(RegistroAuditoria registro, HttpContext httpContext);
    }

    // ── Verificador de Integridad ─────────────────────────────────────────────
    public interface IVerificadorDeIntegridadService
    {
        string GenerarHash(RegistroAuditoria registro);
        Task<bool> VerificarIntegridadAsync(string id);
        Task<IEnumerable<RegistroAuditoria>> DetectarRegistrosCorruptosAsync();
    }
}
