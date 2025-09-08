using uxcomex.Domain.Entities;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Application.Interfaces
{
    public interface IItemPedidosService
    {
        Task<ItemPedidos?> GetByIdAsync(Guid id);
        Task CreateAsync(List<ItemPedidos> itemPedidos);
        Task<bool> UpdateAsync(List<ItemPedidos> itemPedidos);
        Task<bool> DeleteAsync(Guid id);
        Task<List<ItemPedidoViewModel>> GetByIdAsyncViewModel(Guid id);
    }
}
