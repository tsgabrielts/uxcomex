using uxcomex.Domain.Entities;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Domain.Interfaces.Repositories
{
    public interface IPedidosRepository
    {
        Task<IEnumerable<Pedidos>> GetAllAsync();
        Task<Pedidos?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Pedidos produto);
        Task<bool> UpdateAsync(Pedidos produto);
        Task<bool> DeleteAsync(Guid id);
        Task FluxoRegistroPedido(Pedidos pedidos, List<ItemPedidos> itens);
        Task<IEnumerable<PedidoViewModel>> GetAllAsyncViewModel();
        Task<PedidoViewModel> GetByIdAsyncViewModel(Guid id);
        Task<bool> UpdateStatusAsync(Guid ped_id, string status);
    }
}
