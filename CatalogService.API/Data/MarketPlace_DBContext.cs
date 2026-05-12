using Catologo.API.Data.Interface;
using Microsoft.Extensions.Options;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using MongoDB.Driver;

namespace Catalogo.API.Data
{
    public class MarketPlace_DBContext : IMarketPlace_DBContext
    {

        private readonly IMongoDatabase _database;

        public MarketPlace_DBContext(IOptions<DatabaseSettings> settings)
        {
            // El MongoClient es el que "abre la puerta" de Mongo
            var client = new MongoClient(settings.Value.ConnectionString);

            // Seleccionamos la base de datos (CatalogDB)
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

            public IMongoCollection<T> GetCollection<T>(string name)
            {
                return _database.GetCollection<T>(name);
            }
    }
}
