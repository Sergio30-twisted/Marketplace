using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Auditoria_y_Trazabilidad.Domain.Entities
{
    public class RegistroAuditoria
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>Servicio o microservicio que originó el evento</summary>
        public string Servicio { get; set; } = string.Empty;

        /// <summary>Acción ejecutada (ej: CREATE_PRODUCTO, DELETE_CATALOGO)</summary>
        public string Accion { get; set; } = string.Empty;

        /// <summary>Nivel del log: INFO, WARNING, ERROR, CRITICAL</summary>
        public string Nivel { get; set; } = "INFO";

        /// <summary>Mensaje descriptivo del evento</summary>
        public string Mensaje { get; set; } = string.Empty;

        /// <summary>Id del usuario que ejecutó la acción (si aplica)</summary>
        public string? UsuarioId { get; set; }

        /// <summary>Recurso afectado (ej: Producto:abc123)</summary>
        public string? RecursoAfectado { get; set; }

        /// <summary>Datos adicionales en JSON (payload antes/después)</summary>
        public string? Contexto { get; set; }

        /// <summary>IP de origen de la petición</summary>
        public string? IpOrigen { get; set; }

        /// <summary>Correlación para rastrear flujos distribuidos</summary>
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>Hash SHA256 del registro para verificar integridad</summary>
        public string? HashIntegridad { get; set; }

        /// <summary>Indica si el hash fue verificado exitosamente</summary>
        public bool IntegridadVerificada { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
