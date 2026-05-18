using Auditoria_y_Trazabilidad.Application.Interface;
using Auditoria_y_Trazabilidad.Domain.Entities;
using Auditoria_y_Trazabilidad.Infrastructure.Interface;
using System.Security.Cryptography;
using System.Text;

namespace Auditoria_y_Trazabilidad.Application.Service
{
    public class VerificadorDeIntegridadService : IVerificadorDeIntegridadService
    {
        private readonly IAuditoriaRepository _repository;
        private readonly ILogger<VerificadorDeIntegridadService> _logger;

        public VerificadorDeIntegridadService(
            IAuditoriaRepository repository,
            ILogger<VerificadorDeIntegridadService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

     
        /// Genera un hash SHA-256 basado en los campos inmutables del registro.
        /// Se excluyen Id, HashIntegridad e IntegridadVerificada para evitar circularidad.
        public string GenerarHash(RegistroAuditoria registro)
        {
            var contenido = $"{registro.Servicio}|{registro.Accion}|{registro.Nivel}|" +
                            $"{registro.Mensaje}|{registro.UsuarioId}|{registro.RecursoAfectado}|" +
                            $"{registro.Contexto}|{registro.IpOrigen}|{registro.CorrelationId}|" +
                            $"{registro.FechaCreacion:O}";

            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(contenido));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        
        /// Recupera un registro por Id y verifica que su hash no haya sido alterado.
        
        public async Task<bool> VerificarIntegridadAsync(string id)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro is null)
            {
                _logger.LogWarning("Registro {Id} no encontrado para verificación", id);
                return false;
            }

            var hashOriginal = registro.HashIntegridad;
            registro.HashIntegridad = null; // evitar que el hash afecte el recálculo
            var hashRecalculado = GenerarHash(registro);

            var integro = string.Equals(hashOriginal, hashRecalculado, StringComparison.OrdinalIgnoreCase);

            if (!integro)
                _logger.LogError("¡INTEGRIDAD COMPROMETIDA! Registro {Id}: hash esperado={Esperado}, calculado={Calculado}",
                    id, hashOriginal, hashRecalculado);

            return integro;
        }

      
        /// Escanea todos los registros y devuelve los que tienen hash inválido.
        public async Task<IEnumerable<RegistroAuditoria>> DetectarRegistrosCorruptosAsync()
        {
            var todos = await _repository.ObtenerTodosAsync();
            var corruptos = new List<RegistroAuditoria>();

            foreach (var registro in todos)
            {
                var hashOriginal = registro.HashIntegridad;
                registro.HashIntegridad = null;
                var hashRecalculado = GenerarHash(registro);

                if (!string.Equals(hashOriginal, hashRecalculado, StringComparison.OrdinalIgnoreCase))
                {
                    registro.IntegridadVerificada = false;
                    corruptos.Add(registro);
                }
            }

            _logger.LogInformation("Escaneo de integridad completado: {Corruptos}/{Total} registros corruptos",
                corruptos.Count, todos.Count());

            return corruptos;
        }
    }
}
