using Catologo.API.Domain.Entities;

namespace Catalogo.API.Infrastructure.Interface
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Catalogo>> GetAllAsync();
        Task<Catalogo> GetByIdAsync(int id);
        Task CreateAsync(Catalogo catalogo);
        Task UpdateAsync(int id, Catalogo catalogo);
        Task DeleteAsync(int id);
    }
}
