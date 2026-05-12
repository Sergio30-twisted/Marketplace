using Catalogo.Application.Interface;
using Catalogo.Infrastructure.Interface;
using Catalogoentidad = Catalogo.Domain.Entities.Catalogo;
using Catalogo.Protos;

namespace Catalogo.Application.Service
{
    public class ConsultaDeCatalogoService:IConsultaDeCatalogoService
    {
        private readonly IConsultaCatalogoRepository _repository;

        public ConsultaDeCatalogoService(IConsultaCatalogoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Catalogoentidad>> ObtenerTodosLosCatalogos()
        {
            // Lógica para obtener la lista completa de catálogos con productos embebidos
            return await _repository.GetAllAsync();
        }

        public async Task<Catalogoentidad> ObtenerCatalogoPorId(int id)
        {
            // Lógica para obtener un catálogo específico mediante su ID entero
            return await _repository.GetByIdAsync(id);
        }
    }
}
