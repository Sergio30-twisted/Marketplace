using Catalogo.API.Data.Interface;
using Catalogo.API.Domain.Entities;
using Catalogo.API.Infrastructure.Interface;
using MongoDB.Driver;

namespace Catalogo.API.Infrastructure.Repository
{
    public class ProductRepository : IProductoRepository
    {
        private readonly IMongoCollection<Producto> _products;

        public ProductRepository(IMarketPlace_DBContext context)
        {
            // "Products" debe coincidir con el nombre en Compass
            _products = context.GetCollection<Producto>("Products");
        }

        public async Task<IEnumerable<Producto>> GetAllActiveAsync()
        {
            // Equivalente a: SELECT * FROM Products WHERE IsActive = true
            return await _products.Find(p => p.IsActive).ToListAsync();
        }

        public async Task<Producto> CreateAsync(Producto producto)
        {
            // InsertOneAsync persiste el objeto en Compass. 
            // Si producto.Id es null, el driver le asigna un ObjectId automáticamente.
            await _products.InsertOneAsync(producto);
            return producto;
        }

        public async Task<Producto> GetByIdAsync(int id)
        {
            // Filtra por el campo ID que mapeamos como string en la entidad
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(int id, Producto producto)
        {
            // ReplaceOneAsync busca el documento por ID y lo reemplaza por la versión actualizada
            var result = await _products.ReplaceOneAsync(p => p.Id == id, producto);

            // Verificamos que la operación fue reconocida y que se modificó al menos un registro
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
