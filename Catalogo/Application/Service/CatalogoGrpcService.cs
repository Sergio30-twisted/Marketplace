using Catalogo.Application.Interface;
using Catalogo.Infrastructure.Interface;
using Catalogo.Protos;
using Grpc.Core;

namespace Catalogo.Services
{
    public class CatalogoGrpcService : CatalogoService.CatalogoServiceBase
    {
        private readonly IConsultaDeCatalogoService _catalogoService;
        private readonly ICreacionCatalogoRepository _creacionRepository;

        public CatalogoGrpcService(IConsultaDeCatalogoService catalogoService, ICreacionCatalogoRepository creacionRepository)
        {
            _catalogoService = catalogoService;
            _creacionRepository = creacionRepository;
        }

        public override async Task<ProductoResponse> ObtenerProducto(
    GetProductoRequest request, ServerCallContext context)
        {
            var catalogos = await _catalogoService.ObtenerTodosLosCatalogos();

            var producto = catalogos
                .SelectMany(c => c.Productos)
                .FirstOrDefault(p => p.Id.ToString() == request.Id);

            if (producto == null)
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"Producto con ID {request.Id} no encontrado"));

            return new ProductoResponse
            {
                ProductoId = producto.Id.ToString(),
                Nombre = producto.Name,
                Precio = (double)producto.Price,
                Stock = producto.Stock
            };
        }

        public override async Task<UpdateStockResponse> ActualizarStock(
        UpdateStockRequest request, ServerCallContext context)
        {
            var productoId = int.Parse(request.Id);

            await _creacionRepository.ActualizarStockProductoAsync(
                productoId,
                request.Cantidad,
                request.Accion);

            return new UpdateStockResponse { Success = true, Mensaje = "Stock actualizado" };
        }
    }
}