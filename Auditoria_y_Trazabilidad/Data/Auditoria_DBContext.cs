using Auditoria_y_Trazabilidad.Data.Interface;
using MongoDB.Driver;

namespace Auditoria_y_Trazabilidad.Data
{
    public class Auditoria_DBContext : IAuditoria_DBContext
    {
        private readonly IMongoDatabase _database;

        public Auditoria_DBContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("ConnectionStrings:MongoDb").Value;
            var client = new MongoClient(connectionString);
            var databaseName = configuration.GetSection("DatabaseSettings:DatabaseName").Value;
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
