using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace uxcomex.Domain.ViewModel
{
    public class ItemPedidoViewModel
    {

        public Guid? ped_id { get; set; }   
        public string? pro_nome {get; set;}
        [Display(Name = "Produto")]
        [Required(ErrorMessage = "Selecione ao menos 1 item")]
        public Guid? ProdutoId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade deve ser maior ou igual a zero")]
        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "Inclua ao menos 1 item de cada item selecionado")]
        public int? Quantidade { get; set; }
        [Required(ErrorMessage = "O preço unitário não pode estar vazio")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrecoUnitario { get; set; }
        [Required(ErrorMessage = "O subtotal não pode estar vazio")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Subtotal => Quantidade * PrecoUnitario;
    }
}
