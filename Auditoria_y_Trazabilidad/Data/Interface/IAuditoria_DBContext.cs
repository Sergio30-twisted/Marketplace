using MongoDB.Driver;

namespace Auditoria_y_Trazabilidad.Data.Interface
{
    public interface IAuditoria_DBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
