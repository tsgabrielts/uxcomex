using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uxcomex.Domain.Entities
{
    public class ItemPedidos
    {
        public Guid itp_id { get; set; }
        public Guid ped_id { get; set; }
        public Guid pro_id { get; set; }
        public int itp_quantidade { get; set; }
        public decimal itp_preco_unitario { get; set; }
        
    }
}
