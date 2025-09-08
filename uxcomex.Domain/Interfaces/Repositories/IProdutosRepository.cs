using uxcomex.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace uxcomex.Domain.Interfaces.Repositories
{
    public interface IProdutosRepository
    {
        Task<IEnumerable<Produtos>> GetAllAsync();
        Task<Produtos?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Produtos produto);
        Task<bool> UpdateAsync(Produtos produto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateEstoque(List<ItemPedidos> itens);
        Task<bool> UpdateEstoque(List<ItemPedidos> itens, SqlConnection connection, SqlTransaction transaction);

    }
}
