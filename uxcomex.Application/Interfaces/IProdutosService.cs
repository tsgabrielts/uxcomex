using uxcomex.Domain.ViewModel;
using uxcomex.Domain.Entities;

namespace uxcomex.Application.Interfaces
{
    public interface IProdutosService
    {
        Task<IEnumerable<Produtos>> GetAllAsync();
        Task<Produtos?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(ProdutoViewModel produto);
        Task<bool> UpdateAsync(ProdutoViewModel produto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateEstoque(List<ItemPedidos> itens);
    }
}
