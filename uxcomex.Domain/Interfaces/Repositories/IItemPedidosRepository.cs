using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using uxcomex.Domain.Entities;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Domain.Interfaces.Repositories
{
    public interface IItemPedidosRepository
    {
        Task<ItemPedidos?> GetByIdAsync(Guid id);
        Task CreateAsync(List<ItemPedidos> itemPedidos);
        Task CreateAsync(List<ItemPedidos> itemPedidos, SqlConnection connection, SqlTransaction transaction);
        Task<bool> UpdateAsync(List<ItemPedidos> itemPedidos);
        Task<bool> DeleteAsync(Guid id);
        Task<List<ItemPedidoViewModel>> GetByIdAsyncViewModel(Guid id);

    }
}
