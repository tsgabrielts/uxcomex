
using uxcomex.Application.Interfaces;
using uxcomex.Domain.Entities;
using uxcomex.Domain.ViewModel;
using uxcomex.Domain.Interfaces.Repositories;

namespace uxcomex.Application.Services
{
    public class PedidosService : IPedidosService
    {
        private readonly IPedidosRepository _pedidosRepository;

        public PedidosService(IPedidosRepository pedidosRepository)
        {
            _pedidosRepository = pedidosRepository;
        }

        public async Task FluxoRegistroPedido(Pedidos pedido, List<ItemPedidoViewModel> itemPedidoViewModel)
        {
            try
            {
                List<ItemPedidos> itemPedido = itemPedidoViewModel.Select(tp => new ItemPedidos
                {
                    itp_id = Guid.NewGuid(),
                    ped_id = pedido.ped_id,
                    pro_id = tp.ProdutoId.Value,
                    itp_preco_unitario = tp.PrecoUnitario.Value,
                    itp_quantidade = tp.Quantidade.Value,
                }).ToList();

                await _pedidosRepository.FluxoRegistroPedido(pedido, itemPedido);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Pedidos>> GetAllAsync()
        {
            return await _pedidosRepository.GetAllAsync();
        }
        public async Task<Pedidos?> GetByIdAsync(Guid id)
        {
            return await _pedidosRepository.GetByIdAsync(id);
        }
        public async Task<Guid> CreateAsync(Pedidos pedidos)
        {
            return await _pedidosRepository.CreateAsync(pedidos);
        }
        public async Task<bool> UpdateAsync(Pedidos pedidos)
        {
            return await _pedidosRepository.UpdateAsync(pedidos);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _pedidosRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<PedidoViewModel>> GetAllAsyncViewModel()
        {
            return await _pedidosRepository.GetAllAsyncViewModel();
        }

        public async Task<PedidoViewModel> GetByIdAsyncViewModel(Guid id)
        {
            return await _pedidosRepository.GetByIdAsyncViewModel(id);
        }

        public async Task<bool> UpdateStatusAsync(Guid ped_id, string status)
        {
            return await _pedidosRepository.UpdateStatusAsync(ped_id, status);
        }
    }
}
