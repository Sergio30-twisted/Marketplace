

namespace Catalogo.API.Application.Interface
{
    public interface IConsultaDeCatalogoService
    {
        Task<IEnumerable<Catalogo>> ObtenerTodosAsync();


        // Buscar un catálogo específico por su ID entero
        Task<Catalogo> ObtenerPorIdAsync(int id);

        // Buscar catálogos que coincidan con un nombre (ej. "Verduras")
        Task<IEnumerable<Catalogo>> BuscarPorNombreAsync(string nombre);

        // Obtener solo los productos de un catálogo específico
        Task<IEnumerable<Producto>> ObtenerProductosDeCatalogoAsync(int catalogoId);
    }
}
