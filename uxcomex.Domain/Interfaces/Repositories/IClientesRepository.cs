using uxcomex.Domain.Entities;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Domain.Interfaces.Repositories
{
    public interface IClientesRepository
    {
        Task<IEnumerable<ClienteViewModel>> GetAllAsync();
        Task<ClienteViewModel?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Clientes cliente);
        Task<bool> UpdateAsync(Clientes cliente);
        Task<bool> DeleteAsync(Guid id);
    }
}
