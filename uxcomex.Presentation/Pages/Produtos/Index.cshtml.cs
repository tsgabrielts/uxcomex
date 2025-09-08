using uxcomex.Application.Interfaces;
using uxcomex.Presentation.Pages.Shared;
using ProdEntity = uxcomex.Domain.Entities.Produtos;

namespace uxcomex.Presentation.Pages.Produtos
{
    public class IndexModel : BaseIndexModel<ProdEntity>
    {
        private readonly IProdutosService _produtosService;

        public IndexModel(IProdutosService produtosService, ILogger<IndexModel> logger)
            : base(logger)
        {
            _produtosService = produtosService;
        }

        public List<ProdEntity> Produtos { get; set; } = new();

        public async Task OnGetAsync()
        {
            Produtos = await GetDataAsync();
        }

        public override async Task<List<ProdEntity>> GetDataAsync()
        {
            return (await _produtosService.GetAllAsync()).ToList();
        }

        public override string GetEntityName() => "Produto";

        public override string GetDeleteHandler() => "/Produtos?handler=Delete";

        protected override async Task DeleteItemAsync(Guid id)
        {
            await _produtosService.DeleteAsync(id);
        }
    }
}
