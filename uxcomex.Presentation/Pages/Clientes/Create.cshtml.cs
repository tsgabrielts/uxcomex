using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using uxcomex.Application.Interfaces;
using uxcomex.Domain.ViewModel;

namespace uxcomex.Presentation.Pages.Clients
{
    public class CreateModel : PageModel
    {

        private readonly IClienteService _clienteService;

        [BindProperty]
        public ClienteViewModel Cliente { get; set; } = new ClienteViewModel();
        public bool IsEdit => Cliente.cli_id != Guid.Empty;


        public CreateModel(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id.HasValue && id.Value != Guid.Empty)
            {
                try
                {
                    var clienteExistente = await _clienteService.GetClienteByIdAsync(id.Value);
                    if (clienteExistente == null)
                    {
                        TempData["ErrorMessage"] = "Cliente não encontrado.";
                        return RedirectToPage("/Clientes/Index");
                    }

                    Cliente = new ClienteViewModel
                    {
                        cli_id = clienteExistente.cli_id,
                        cli_nome = clienteExistente.cli_nome,
                        cli_email = clienteExistente.cli_email,
                        cli_telefone = clienteExistente.cli_telefone
                    };
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Erro ao carregar cliente: " + ex.Message;
                    return RedirectToPage("/Clientes/Index");
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

                    await _clienteService.UpdateClienteAsync(Cliente);

                    TempData["SuccessMessage"] = $"Cliente '{Cliente.cli_nome}' atualizado com sucesso!";
                }
                else
                {
                    await _clienteService.CreateClienteAsync(Cliente);

                    TempData["SuccessMessage"] = "Cliente criado com sucesso!";
                }
                return RedirectToPage("/Clientes/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao criar cliente: " + ex.Message;
                return Page();
            }
        }
    }



}
