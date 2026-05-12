using Catalogoentidad = Catalogo.Domain.Entities.Catalogo;

namespace Catalogo.Application.Interface
{
    public interface IConsultaDeCatalogoService
    {
        Task<IEnumerable<Catalogoentidad>> ObtenerTodosLosCatalogos();
        Task<Catalogoentidad> ObtenerCatalogoPorId(int id);
    }
}
