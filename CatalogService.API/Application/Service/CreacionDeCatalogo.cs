using Catalogo.API.Domain.Entities;

namespace Catalogo.API.Application.Service
{
    public class CreacionDeCatalogo
    {
        public async Task<Catalogo> CrearCatalogo(Catalogo catalogo)
        {
            var existentes = await _repository.GetAllAsync();
            // Generamos el ID entero para el documento principal
            catalogo.Id = existentes.Any() ? existentes.Max(c => c.Id) + 1 : 1;

            await _repository.CreateAsync(catalogo);
            return catalogo;
        }
    }
}
