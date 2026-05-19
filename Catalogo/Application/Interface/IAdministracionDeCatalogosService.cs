namespace Catalogo.Application.Interface
{
    public interface IAdministracionDeCatalogosService
    {
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, Catalogo.Domain.Entities.Catalogo catalogoActualizado);
        Task CrearAsync(Catalogo.Domain.Entities.Catalogo catalogo);
    }

}
