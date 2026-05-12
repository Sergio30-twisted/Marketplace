
using Catalogoentidad = Catalogo.Domain.Entities.Catalogo;

namespace Catalogo.Infrastructure.Interface
{
    public interface IConsultaCatalogoRepository
    {
        Task<IEnumerable<Catalogoentidad>> GetAllAsync();
        Task<Catalogoentidad> GetByIdAsync(int id);
    }
}
