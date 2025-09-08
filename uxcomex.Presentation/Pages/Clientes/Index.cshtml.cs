using uxcomex.Application.Interfaces;
using uxcomex.Domain.ViewModel;
using uxcomex.Presentation.Pages.Shared;

namespace uxcomex.Presentation.Pages.Clientes
{
    public class IndexModel : BaseIndexModel<ClienteViewModel>
    {
        private readonly IClienteService _clienteService;

        public IndexModel(IClienteService clienteService, ILogger<IndexModel> logger)
            : base(logger)
        {
            _clienteService = clienteService;
        }

        public List<ClienteViewModel> Clientes { get; set; } = new();

        public async Task OnGetAsync()
        {
            Clientes = await GetDataAsync();
        }

        public override async Task<List<ClienteViewModel>> GetDataAsync()
        {
            return (await _clienteService.GetAllClientesAsync()).ToList();
        }

        public override string GetEntityName() => "Cliente";

        public override string GetDeleteHandler() => "/Clientes?handler=Delete";

        protected override async Task DeleteItemAsync(Guid id)
        {
            await _clienteService.DeleteClienteAsync(id);
        }
    }
}
