using Catalogo.API.Domain.Entities;

namespace Catalogo.API.Application.Interface.Producto
{
    public interface IProductoService
    {
        Task<bool> AgregarProductoACatalogoAsync(int catalogoId, Producto nuevoProducto);
        Task<bool> ActualizarProductoEnCatalogoAsync(int catalogoId, Producto productoEditado);
        Task<bool> EliminarProductoDeCatalogoAsync(int catalogoId, int productoId);
    }
}
