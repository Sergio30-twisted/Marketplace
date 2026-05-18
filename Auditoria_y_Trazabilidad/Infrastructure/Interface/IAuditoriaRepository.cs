using Auditoria_y_Trazabilidad.Domain.Entities;

namespace Auditoria_y_Trazabilidad.Infrastructure.Interface
{
    public interface IAuditoriaRepository
    {
        Task InsertarRegistroAsync(RegistroAuditoria registro);
        Task<IEnumerable<RegistroAuditoria>> ObtenerTodosAsync();
        Task<IEnumerable<RegistroAuditoria>> ObtenerPorServicioAsync(string servicio);
        Task<IEnumerable<RegistroAuditoria>> ObtenerPorCorrelationIdAsync(string correlationId);
        Task<IEnumerable<RegistroAuditoria>> ObtenerPorFechasAsync(DateTime desde, DateTime hasta);
        Task<RegistroAuditoria?> ObtenerPorIdAsync(string id);
    }
}
