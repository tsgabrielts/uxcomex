using uxcomex.Application.Interfaces;
using uxcomex.Domain.Entities;
using uxcomex.Domain.Interfaces.Repositories;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Application.Services
{
    public class ItemPedidosService : IItemPedidosService
    {
        private readonly IItemPedidosRepository _itemPedidosRepository;
        
        public ItemPedidosService(IItemPedidosRepository itemPedidosRepository)
        {
            _itemPedidosRepository = itemPedidosRepository;
        }

        public async Task<ItemPedidos?> GetByIdAsync(Guid id)
        {
            return await _itemPedidosRepository.GetByIdAsync(id);
        }
        public async Task CreateAsync(List<ItemPedidos> itemPedidos)
        {
            await _itemPedidosRepository.CreateAsync(itemPedidos);
        }
        public async Task<bool> UpdateAsync(List<ItemPedidos> itemPedidos)
        {
            return await _itemPedidosRepository.UpdateAsync(itemPedidos);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _itemPedidosRepository.DeleteAsync(id);
        }

        public async Task<List<ItemPedidoViewModel>> GetByIdAsyncViewModel(Guid id)
        {
            return await _itemPedidosRepository.GetByIdAsyncViewModel(id);
        }

    }
}
