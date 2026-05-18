using Auditoria_y_Trazabilidad.Application.Interface;
using Auditoria_y_Trazabilidad.Domain.Entities;

namespace Auditoria_y_Trazabilidad.Application.Service
{
    public class EnriquecedorDeContextoService : IEnriquecedorDeContextoService
    {
        public RegistroAuditoria Enriquecer(RegistroAuditoria registro, HttpContext httpContext)
        {
            // IP de origen (considera proxies)
            registro.IpOrigen = httpContext.Connection.RemoteIpAddress?.ToString()
                ?? httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? "desconocida";

            // CorrelationId: reutilizar el del header entrante si existe
            var correlationHeader = httpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(correlationHeader))
                registro.CorrelationId = correlationHeader;

            // Usuario autenticado si está disponible
            if (string.IsNullOrWhiteSpace(registro.UsuarioId))
            {
                var userId = httpContext.User?.FindFirst("sub")?.Value
                    ?? httpContext.User?.Identity?.Name;
                if (!string.IsNullOrWhiteSpace(userId))
                    registro.UsuarioId = userId;
            }

            // Timestamp siempre en UTC
            registro.FechaCreacion = DateTime.UtcNow;

            return registro;
        }
    }
}
