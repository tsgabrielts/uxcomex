namespace uxcomex.Domain.Entities
{
    public class Clientes
    {

            public Guid cli_id { get; set; }
            public string cli_nome { get; set; } = string.Empty;
            public string? cli_email { get; set; } = string.Empty;
            public string? cli_telefone { get; set; }
            public DateTime cli_data_cadastro { get; set; }
    }
}
