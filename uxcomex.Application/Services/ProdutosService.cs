using uxcomex.Application.Interfaces;
using uxcomex.Domain.Entities;
using uxcomex.Domain.Interfaces.Repositories;
using uxcomex.Domain.ViewModel;
namespace uxcomex.Application.Services
{
    public class ProdutosService : IProdutosService
    {
        private readonly IProdutosRepository _produtoRepository;

        public ProdutosService(IProdutosRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }
        public async Task<IEnumerable<Produtos>> GetAllAsync() { 
            return await _produtoRepository.GetAllAsync();
        }
        public async Task<Produtos?> GetByIdAsync(Guid id) { 
            return await _produtoRepository.GetByIdAsync(id);
        }
        public async Task<Guid> CreateAsync(ProdutoViewModel produtoViewModel) {
            Produtos produto = ConverteViewModelEmEntidade(produtoViewModel);
            return await _produtoRepository.CreateAsync(produto);
        }
        public async Task<bool> UpdateAsync(ProdutoViewModel produtoViewModel) {
            Produtos produto = ConverteViewModelEmEntidade(produtoViewModel);
            return await _produtoRepository.UpdateAsync(produto);
        }
        public async Task<bool> DeleteAsync(Guid id) { 
            return await _produtoRepository.DeleteAsync(id);
        }
        public async Task<bool> UpdateEstoque(List<ItemPedidos> itens)
        {
            return await _produtoRepository.UpdateEstoque(itens);
        }

        private Produtos ConverteViewModelEmEntidade(ProdutoViewModel produtoViewModel)
        {
            var produto = new Produtos()
            {
                pro_id = produtoViewModel.pro_id != Guid.Empty ? produtoViewModel.pro_id : Guid.NewGuid(),
                pro_nome = produtoViewModel.pro_nome,
                pro_descricao = produtoViewModel.pro_descricao,
                pro_quantidade_estoque = produtoViewModel.pro_quantidade_estoque.Value,
                pro_valor = produtoViewModel.pro_valor.Value,
            };

            return produto;

        }
    }
}
