using Catalogo.Application.Interface;
using Catalogo.Data;
using Catalogo.Model;
using Microsoft.Extensions.Options;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalogo.Application.Service
{
    public class GestorDeCategoriaService:IGestorDeCategoriaService
    {
        private readonly IMongoCollection<Catalogo.Domain.Entities.Catalogo> _catalogosCollection;

        public GestorDeCategoriaService(IMarketPlace_DBContext context)
        {
            // CAMBIO CRÍTICO: Debe ser "Products", no "Catalogo"
            _catalogosCollection = context.GetCollection<Catalogo.Domain.Entities.Catalogo>("Products");
        }

        public async Task<List<Catalogo.Domain.Entities.Catalogo>> GetByCategoriaAsync(CategoriaCatalogo categoria)
        {
            // Convertimos el Enum a string ("Vestimenta") para que coincida con Compass
            string nombreABuscar = categoria.ToString();

            // Filtro exacto para el campo "Categoria" (con C mayúscula como en tu foto)
            var filter = Builders<Catalogo.Domain.Entities.Catalogo>.Filter.Eq("Categoria", nombreABuscar);

            return await _catalogosCollection.Find(filter).ToListAsync();
        }

    }
}
