
using System.ComponentModel.DataAnnotations;

namespace uxcomex.Domain.ViewModel
{
    public class ProdutoViewModel
    {
        public Guid pro_id { get; set; } = Guid.Empty;
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string pro_nome { get; set; } = string.Empty;
        public string? pro_descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O valor é obrigatório")]
        public decimal? pro_valor { get; set; } = null;

        [Required(ErrorMessage = "A quantidade em estoque obrigatória")]
        public decimal? pro_quantidade_estoque { get; set; } = null;
    }
}
