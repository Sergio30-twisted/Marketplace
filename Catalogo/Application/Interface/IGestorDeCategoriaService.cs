using Catalogo.Model;

namespace Catalogo.Application.Interface
{
    public interface IGestorDeCategoriaService
    {
        Task<List<Catalogo.Domain.Entities.Catalogo>> GetByCategoriaAsync(CategoriaCatalogo categoria);
    }
}
