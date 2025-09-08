using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace uxcomex.Presentation.Pages.Shared
{
    public abstract class BaseIndexModel<T> : PageModel where T : class
    {
        protected readonly ILogger<BaseIndexModel<T>> _logger;

        public BaseIndexModel(ILogger<BaseIndexModel<T>> logger)
        {
            _logger = logger;
        }

        public abstract Task<List<T>> GetDataAsync();
        public abstract string GetEntityName();
        public abstract string GetDeleteHandler();

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                await DeleteItemAsync(id);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir {GetEntityName()} com ID {id}");
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        protected abstract Task DeleteItemAsync(Guid id);
    }
}
