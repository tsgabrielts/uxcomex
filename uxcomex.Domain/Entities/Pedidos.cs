using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uxcomex.Domain.Entities
{
    public class Pedidos
    {
        public Guid ped_id { get; set; }
        public DateTime ped_data { get; set; }
        public decimal ped_valor_total { get; set; }
        public string? ped_status { get; set; }
        public Guid cli_id { get; set; }


    }
}
