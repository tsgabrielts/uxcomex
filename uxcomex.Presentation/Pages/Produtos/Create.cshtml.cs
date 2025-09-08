using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using uxcomex.Application.Interfaces;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Presentation.Pages.Produtos
{
    public class CreateModel : PageModel
    {
        private readonly IProdutosService _produtosService;

        public CreateModel(IProdutosService produtosService)
        {
            _produtosService = produtosService;
        }

        [BindProperty]
        public ProdutoViewModel Produto { get; set; } = new ProdutoViewModel();
        public bool IsEdit => Produto.pro_id != Guid.Empty;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id.HasValue && id.Value != Guid.Empty)
            {
                try
                { 
                    var produto = await _produtosService.GetByIdAsync(id.Value);
                        if (produto == null)
                        {
                            TempData["ErrorMessage"] = "Produto não encontrado.";
                            return RedirectToPage("/Produtos/Index");
                        }
                    

                    Produto = new ProdutoViewModel { 
                        pro_id = produto.pro_id, 
                        pro_nome = produto.pro_nome, 
                        pro_descricao = produto.pro_descricao, 
                        pro_quantidade_estoque = produto.pro_quantidade_estoque,
                        pro_valor = produto.pro_valor,
                    };

                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Erro ao carregar produto: " + ex.Message;
                    return RedirectToPage("/Produtos/Index");
                }


            }
                return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (IsEdit)
                {

                    await _produtosService.UpdateAsync(Produto);

                    TempData["SuccessMessage"] = $"Produto '{Produto.pro_nome}' atualizado com sucesso!";
                }
                else
                {
                    Produto.pro_id = Guid.NewGuid();

                    await _produtosService.CreateAsync(Produto);

                    TempData["SuccessMessage"] = "Produto criado com sucesso!";
                }
                return RedirectToPage("/Produtos/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao criar produto: " + ex.Message;
                return Page();
            }
        }
    }

}
