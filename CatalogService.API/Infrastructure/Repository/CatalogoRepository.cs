using Catologo.API.Data.Interface;
using Catologo.API.Domain.Entities;
using Catologo.API.Infrastructure.Interface;
using MongoDB.Driver;

namespace Catalogo.API.Infrastructure.Repository
{
    public class CatalogoRepository : ICatalogoRepository
    {
        private readonly IMongoCollection<Catalogo> _catalogos;

        public CatalogoRepository(IMarketPlace_DBContext context)
        {
            // La colección se llamará "Catalogos" en MongoDB Compass
            _catalogos = context.GetCollection<Catalogo>("Products");
        }

        public async Task<IEnumerable<Catalogo>> GetAllAsync() =>
         await _catalogos.Find(_ => true).ToListAsync();

        public async Task<Catalogo> GetByIdAsync(int id) =>
            await _catalogos.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Catalogo catalogo) =>
            await _catalogos.InsertOneAsync(catalogo);

        public async Task UpdateAsync(int id, Catalogo catalogo) =>
            await _catalogos.ReplaceOneAsync(c => c.Id == id, catalogo);

        public async Task DeleteAsync(int id) =>
            await _catalogos.DeleteOneAsync(c => c.Id == id);
    }
}
