using MongoDB.Driver;

namespace Catalogo.API.Data.Interface
{
    public interface IMarketPlace_DBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
