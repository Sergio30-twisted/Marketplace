using Catalogo.Data;
using Catalogo.Domain.Entities;
using Catalogo.Infrastructure.Interface;
using MongoDB.Driver;
using Catalogoentidad = Catalogo.Domain.Entities.Catalogo;

namespace Catalogo.Infrastructure.Repository
{
    public class ConsultaCatalogoRepository : IConsultaCatalogoRepository
    {
        private readonly IMongoCollection<Catalogoentidad> _context;

        public ConsultaCatalogoRepository(IMarketPlace_DBContext context)
        {
            // Se guarda en la colección "Products" como solicitaste anteriormente
            _context = context.GetCollection<Catalogoentidad>("Products");
        }


        public async Task<IEnumerable<Catalogoentidad>> GetAllAsync()
        {
            // Retorna todos los catálogos con sus productos embebidos
            return await _context.Find(_ => true).ToListAsync();
        }

        public async Task<Catalogoentidad> GetByIdAsync(int id)
        {
            // Busca por el ID entero definido en tu entidad de dominio
            return await _context.Find(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}
