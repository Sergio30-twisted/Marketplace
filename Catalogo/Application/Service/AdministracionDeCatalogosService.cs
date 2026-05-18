using Catalogo.Application.Interface;
using Catalogo.Data;
using MongoDB.Driver;

namespace Catalogo.Application.Service
{
    public class AdministracionDeCatalogosService:IAdministracionDeCatalogosService
    {
        private readonly IMongoCollection<Catalogo.Domain.Entities.Catalogo> _catalogosCollection;

        public AdministracionDeCatalogosService(IMarketPlace_DBContext context)
        {
            // Usamos "Products" porque así se llama tu colección en MongoDB Compass
            _catalogosCollection = context.GetCollection<Catalogo.Domain.Entities.Catalogo>("Products");
        }

        // ELIMINAR
        public async Task<bool> DeleteAsync(int id)
        {
            // Buscamos por el Id (que mapea a _id en Mongo) y borramos
            var result = await _catalogosCollection.DeleteOneAsync(x => x.Id == id);

            // Retorna true si Mongo confirmó la operación y si efectivamente borró algo
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        // ACTUALIZAR (EDITAR)
        public async Task<bool> UpdateAsync(int id, Catalogo.Domain.Entities.Catalogo catalogoActualizado)
        {
            // Reemplazamos el documento completo que coincida con el ID
            var result = await _catalogosCollection.ReplaceOneAsync(x => x.Id == id, catalogoActualizado);

            // Retorna true si hubo un documento modificado o encontrado para reemplazar
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
