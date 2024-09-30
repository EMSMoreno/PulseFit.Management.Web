using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class RecoverPasswordViewModel
    {
        // O atributo [Required] indica que o campo é obrigatório e deve ser preenchido.
        // Se o campo estiver vazio, uma mensagem de erro será gerada.
        [Required(ErrorMessage = "O email é obrigatório.")]
        // O atributo [EmailAddress] valida que o valor inserido é um endereço de email válido.
        // Garante que o formato do email esteja correto (por exemplo, user@example.com).
        [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
        // Propriedade que armazena o endereço de email do utilizador.
        public string Email { get; set; }
    }
}