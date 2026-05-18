using Catalogoentidad = Catalogo.Domain.Entities.Catalogo;


namespace Catalogo.Infrastructure.Interface
{
    public interface ICreacionCatalogoRepository
    {

        Task InsertarNuevoCatalogoAsync(Catalogoentidad catalogo);
        Task ActualizarIsActivoProductoAsync(int productoId, bool isActive);
        Task ActualizarStockProductoAsync(int productoId, int cantidad, string accion);
    }
}
