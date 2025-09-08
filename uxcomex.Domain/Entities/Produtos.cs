

namespace uxcomex.Domain.Entities
{
    public class Produtos
    {
        public Guid pro_id { get; set; }
        public string pro_nome { get; set; } = string.Empty;
        public string? pro_descricao { get; set; }
        public decimal pro_valor { get; set; }
        public decimal pro_quantidade_estoque { get; set; }

    }
}
