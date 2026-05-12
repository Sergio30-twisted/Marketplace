using Catologo.API.Application.Interface.Producto;
using Catologo.API.Domain.Entities;
using Catalogo.API.Infrastructure.Interface;

namespace Catalogo.API.Application.Service
{
    public class ProductoService:IProductoService
    {
        private readonly IProductoRepository _repository;


        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> ActualizarProductoEnCatalogoAsync(int catalogoId, Producto productoEditado)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AgregarProductoACatalogoAsync(int catalogoId, Producto nuevoProducto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarProductoDeCatalogoAsync(int catalogoId, int productoId)
        {
            throw new NotImplementedException();
        }
    }
}
