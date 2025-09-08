
using System.ComponentModel.DataAnnotations;

namespace uxcomex.Domain.ViewModel
{
    public class ClienteViewModel
    {
        public Guid cli_id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string cli_nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Digite um email válido")]
        [StringLength(150, ErrorMessage = "O email deve ter no máximo 150 caracteres")]
        public string cli_email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [Phone(ErrorMessage = "Digite um telefone válido")]
        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres")]
        public string cli_telefone { get; set; } = string.Empty;
    }
}
