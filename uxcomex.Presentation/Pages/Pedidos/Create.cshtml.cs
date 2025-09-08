using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using uxcomex.Domain.ViewModel;
using System.ComponentModel.DataAnnotations;
using uxcomex.Application.Interfaces;
using ProdEntity = uxcomex.Domain.Entities.Produtos;
using PedEntity = uxcomex.Domain.Entities.Pedidos;

namespace uxcomex.Presentation.Pages.Pedidos
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;
        private readonly IClienteService _clienteService;
        private readonly IProdutosService _produtosService;
        private readonly IPedidosService _pedidosService;

        public CreateModel(IClienteService clienteService, IProdutosService produtosService, IPedidosService pedidosService, ILogger<ErrorModel> logger)
        {
            _clienteService = clienteService;
            _produtosService = produtosService;
            _pedidosService = pedidosService;
            _logger = logger;
        }

        [BindProperty]
        public NovoPedidoViewModel NovoPedido { get; set; } = new();

        [BindProperty]
        public List<ItemPedidoViewModel> Itens { get; set; } = new();

        public List<SelectListItem> ClientesSelect { get; set; } = new();
        public List<SelectListItem> ProdutosSelect { get; set; } = new();
        public List<ProdEntity> ProdutosDisponiveis { get; set; }

        public async Task OnGetAsync()
        {
            await FillSelects();
        }

        private async Task FillSelects()
        {
            var clientes = await _clienteService.GetAllClientesAsync();
            ClientesSelect = clientes
                .Select(c => new SelectListItem
                {
                    Value = c.cli_id.ToString(),
                    Text = c.cli_nome
                })
                .ToList();

            ProdutosDisponiveis = (await _produtosService.GetAllAsync()).ToList();
            ProdutosSelect = ProdutosDisponiveis.Select(x => new SelectListItem
            {
                Value = x.pro_id.ToString(),
                Text = $"{x.pro_nome} - {x.pro_valor:C} (Estoque: {x.pro_quantidade_estoque})"
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await FillSelects();
                return Page();
            }

            try
            {
                if (Itens.Any())
                {

                    PedEntity pedido = new PedEntity() { cli_id = NovoPedido.ClienteId.Value, ped_valor_total = Itens.Sum(x => x.Subtotal.Value), ped_status = NovoPedido.Status, ped_id = Guid.NewGuid() };

                    await _pedidosService.FluxoRegistroPedido(pedido, Itens);

                    TempData["Sucesso"] = $"Pedido criado com sucesso!";
                } else
                {
                    TempData["Erro"] = "Insira pelo menos um item";
                    await FillSelects();
                    return Page();
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pedido");
                TempData["Erro"] = ex.Message;

                await FillSelects();
                return Page();
            }
            return RedirectToPage("/Pedidos/Index");
        }

        public async Task<JsonResult> OnGetProdutoInfo(string produtoId)
        {
            Guid guidId = Guid.Parse(produtoId);
            var produto = await _produtosService.GetByIdAsync(guidId);
            if (produto != null)
            {
                return new JsonResult(new
                {
                    preco = produto.pro_valor,
                    estoque = produto.pro_quantidade_estoque,
                    nome = produto.pro_nome
                });
            }
            return new JsonResult(null);
        }
    }

    // ViewModels
    public class NovoPedidoViewModel
    {
        [Required(ErrorMessage = "Selecione um cliente")]
        [Display(Name = "Cliente")]
        public Guid? ClienteId { get; set; }

        [Display(Name = "Data do Pedido")]
        public DateTime DataPedido { get; set; } = DateTime.Now;

        [Display(Name = "Status")]
        public string Status { get; set; } = "Novo";
    }
}