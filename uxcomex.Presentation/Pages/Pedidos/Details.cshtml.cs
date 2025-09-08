using uxcomex.Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using uxcomex.Application.Interfaces;

namespace uxcomex.Presentation.Pages.Pedidos
{
    public class DetailsModel : PageModel
    {
        private readonly IPedidosService _pedidosService;
        private readonly IItemPedidosService _itemPedidosService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IPedidosService pedidosService, ILogger<DetailsModel> logger, IItemPedidosService itemPedidosService)
        {
            _pedidosService = pedidosService;
            _itemPedidosService = itemPedidosService;
            _logger = logger;
        }

        [BindProperty]
        public PedidoViewModel Pedido { get; set; }
        [BindProperty]
        public List<ItemPedidoViewModel> Produtos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Pedido = await _pedidosService.GetByIdAsyncViewModel(id);
                Produtos = await _itemPedidosService.GetByIdAsyncViewModel(id);
                if (Pedido == null)
                {
                    _logger.LogWarning($"Pedido with ID {id} not found");
                    return NotFound();
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading pedido details for ID {id}");
                TempData["Erro"] = "Erro ao carregar detalhes do pedido.";
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(Guid id, string status)
        {
            await _pedidosService.UpdateStatusAsync(id, status);
            return new JsonResult(new { success = true });
        }
    }
}