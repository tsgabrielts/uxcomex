
using uxcomex.Application.Interfaces;
using uxcomex.Presentation.Pages.Shared;
using uxcomex.Domain.ViewModel;


namespace uxcomex.Presentation.Pages.Pedidos
{
    public class IndexModel : BaseIndexModel<PedidoViewModel>
    {
        private readonly IPedidosService _pedidosService;

        public IndexModel(IPedidosService pedidosService, ILogger<IndexModel> logger)
            : base(logger)
        {
            _pedidosService = pedidosService;
        }

        public List<PedidoViewModel> Pedidos { get; set; } = new();

        public async Task OnGetAsync()
        {
            Pedidos = await GetDataAsync();
        }

        public override async Task<List<PedidoViewModel>> GetDataAsync()
        {
            return (await _pedidosService.GetAllAsyncViewModel()).ToList();
        }

        public override string GetEntityName() => "Pedido";

        public override string GetDeleteHandler() => "/Pedidos?handler=Delete";

        protected override async Task DeleteItemAsync(Guid id)
        {
            await _pedidosService.DeleteAsync(id);
        }
    }
}
