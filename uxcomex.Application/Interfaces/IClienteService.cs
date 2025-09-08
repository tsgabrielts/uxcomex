using uxcomex.Domain.Entities;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Application.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteViewModel>> GetAllClientesAsync();
        Task<ClienteViewModel?> GetClienteByIdAsync(Guid id);
        Task<Guid> CreateClienteAsync(ClienteViewModel cliente);
        Task<bool> DeleteClienteAsync(Guid id);
        Task<bool> UpdateClienteAsync(ClienteViewModel cliente);
    }
}
