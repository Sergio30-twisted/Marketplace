using Auditoria_y_Trazabilidad.Data.Interface;
using Auditoria_y_Trazabilidad.Domain.Entities;
using Auditoria_y_Trazabilidad.Infrastructure.Interface;
using MongoDB.Driver;

namespace Auditoria_y_Trazabilidad.Infrastructure.Repository
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly IMongoCollection<RegistroAuditoria> _collection;
        private const string ColeccionNombre = "RegistrosAuditoria";

        public AuditoriaRepository(IAuditoria_DBContext context)
        {
            _collection = context.GetCollection<RegistroAuditoria>(ColeccionNombre);

            // Índices para consultas frecuentes
            var indexServicio = Builders<RegistroAuditoria>.IndexKeys.Ascending(r => r.Servicio);
            var indexCorrelation = Builders<RegistroAuditoria>.IndexKeys.Ascending(r => r.CorrelationId);
            var indexFecha = Builders<RegistroAuditoria>.IndexKeys.Descending(r => r.FechaCreacion);

            _collection.Indexes.CreateMany(new[]
            {
                new CreateIndexModel<RegistroAuditoria>(indexServicio),
                new CreateIndexModel<RegistroAuditoria>(indexCorrelation),
                new CreateIndexModel<RegistroAuditoria>(indexFecha)
            });
        }

        public async Task InsertarRegistroAsync(RegistroAuditoria registro)
        {
            await _collection.InsertOneAsync(registro);
        }

        public async Task<IEnumerable<RegistroAuditoria>> ObtenerTodosAsync()
        {
            return await _collection
                .Find(_ => true)
                .SortByDescending(r => r.FechaCreacion)
                .Limit(500)
                .ToListAsync();
        }

        public async Task<IEnumerable<RegistroAuditoria>> ObtenerPorServicioAsync(string servicio)
        {
            var filtro = Builders<RegistroAuditoria>.Filter.Eq(r => r.Servicio, servicio);
            return await _collection
                .Find(filtro)
                .SortByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<RegistroAuditoria>> ObtenerPorCorrelationIdAsync(string correlationId)
        {
            var filtro = Builders<RegistroAuditoria>.Filter.Eq(r => r.CorrelationId, correlationId);
            return await _collection.Find(filtro).ToListAsync();
        }

        public async Task<IEnumerable<RegistroAuditoria>> ObtenerPorFechasAsync(DateTime desde, DateTime hasta)
        {
            var filtro = Builders<RegistroAuditoria>.Filter.And(
                Builders<RegistroAuditoria>.Filter.Gte(r => r.FechaCreacion, desde),
                Builders<RegistroAuditoria>.Filter.Lte(r => r.FechaCreacion, hasta)
            );
            return await _collection
                .Find(filtro)
                .SortByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<RegistroAuditoria?> ObtenerPorIdAsync(string id)
        {
            var filtro = Builders<RegistroAuditoria>.Filter.Eq(r => r.Id, id);
            return await _collection.Find(filtro).FirstOrDefaultAsync();
        }
    }
}
