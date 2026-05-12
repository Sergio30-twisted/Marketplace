using MongoDB.Driver;

namespace Catalogo.Data
{
    public interface IMarketPlace_DBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
