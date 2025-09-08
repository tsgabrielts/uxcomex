using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uxcomex.Domain.ViewModel
{
    public class PedidoViewModel
    {
        [Required]
        [Display(Name  = "Nº do Pedido")]
        public Guid  ped_id { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public string cli_nome { get; set; }

        [Required]
        [Display(Name = "Valor Total=")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public string ped_valor_total { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string ped_status { get; set; }

        [Required]
        [Display(Name = "Status")]
        public DateTime ped_data { get; set; }


    }
}
