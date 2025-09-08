using uxcomex.Domain.ViewModel;
using uxcomex.Domain.Entities;

namespace uxcomex.Application.Interfaces
{
    public interface IPedidosService
    {
        Task<IEnumerable<Pedidos>> GetAllAsync();
        Task<Pedidos?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Pedidos pedidos);
        Task<bool> UpdateAsync(Pedidos pedidos);
        Task<bool> DeleteAsync(Guid id);
        Task FluxoRegistroPedido(Pedidos pedido, List<ItemPedidoViewModel> itemPedidoViewModel);
        Task<IEnumerable<PedidoViewModel>> GetAllAsyncViewModel();
        Task<PedidoViewModel> GetByIdAsyncViewModel(Guid id);
        Task<bool> UpdateStatusAsync(Guid ped_id, string status);

    }
}
