using Catalogo.API.Application.Interface.Catalogo;
using Catalogo.API.Domain.Entities;
using Catalogo.API.Infrastructure.Interface;

namespace Catalogo.API.Application.Service;

public class ConsultaDeCatalogoService:IConsultaDeCatalogoService
{
    private readonly IProductoRepository _repository;

    public ConsultaDeCatalogoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Catalogo>> ConsultarCatalogos() =>
        await _repository.GetAllAsync();

   
}
