using MongoDB.Driver;

namespace Catalogo.Data
{
    public class MarketPlace_DBContext:IMarketPlace_DBContext
    {
        private readonly IMongoDatabase _database;

        public MarketPlace_DBContext(IConfiguration configuration)
        {
            // 1. Conexión al servidor
            var connectionString = configuration.GetSection("ConnectionStrings:MongoDb").Value;
            var client = new MongoClient(connectionString);

            // 2. Conexión a la base de datos específica
            // Aseguramos que lea "DatabaseSettings" y luego "DatabaseName"
            var databaseName = configuration.GetSection("DatabaseSettings:DatabaseName").Value;

            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
