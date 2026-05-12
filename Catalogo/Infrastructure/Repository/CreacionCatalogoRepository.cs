using Catalogo.Data;
using Catalogo.Infrastructure.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalogo.Infrastructure.Repository
{
    public class CreacionCatalogoRepository : ICreacionCatalogoRepository
    {
        private readonly IMongoCollection<Catalogo.Domain.Entities.Catalogo> _context;

        public CreacionCatalogoRepository(IMarketPlace_DBContext context)
        {
            // Apuntamos a la misma colección "Products" que definiste en Compass
            _context = context.GetCollection<Catalogo.Domain.Entities.Catalogo>("Products");
        }

        public async Task ActualizarIsActivoProductoAsync(int productoId, bool isActive)
        {
            var filter = Builders<Catalogo.Domain.Entities.Catalogo>
                .Filter.ElemMatch(c => c.Productos, p => p.Id == productoId);

            var update = Builders<Catalogo.Domain.Entities.Catalogo>
                .Update.Set("productos.$[elem].isActive", isActive);

            var arrayFilters = new List<ArrayFilterDefinition>
        {
        new BsonDocumentArrayFilterDefinition<BsonDocument>(
            new BsonDocument("elem._id", new BsonInt32(productoId))) 
        };

            var options = new UpdateOptions { ArrayFilters = arrayFilters };

            await _context.UpdateOneAsync(filter, update, options);
        }

        public async Task ActualizarStockProductoAsync(int productoId, int cantidad, string accion)
        {
            var catalogos = await _context.Find(_ => true).ToListAsync();
            var catalogo = catalogos.FirstOrDefault(c => c.Productos.Any(p => p.Id == productoId));

            if (catalogo == null) return;

            var producto = catalogo.Productos.First(p => p.Id == productoId);

            if (accion == "RESERVE")
                producto.Stock -= cantidad;
            else if (accion == "RELEASE")
                producto.Stock += cantidad;

            if (producto.Stock <= 0)
                producto.IsActive = false;

            var filter = Builders<Catalogo.Domain.Entities.Catalogo>
                .Filter.Eq(c => c.Id, catalogo.Id);

            var update = Builders<Catalogo.Domain.Entities.Catalogo>
                .Update.Set(c => c.Productos, catalogo.Productos);

            await _context.UpdateOneAsync(filter, update);
        }

        public async Task InsertarNuevoCatalogoAsync(Catalogo.Domain.Entities.Catalogo entidad)
        {
            // Lógica para simular Identity: busca el ID más alto y le suma 1
            var ultimoCatalogo = await _context.Find(new BsonDocument())
                                                 .SortByDescending(x => x.Id)
                                                 .FirstOrDefaultAsync();

            entidad.Id = (ultimoCatalogo == null) ? 1 : ultimoCatalogo.Id + 1;

            await _context.InsertOneAsync(entidad);
        }

    }
}
