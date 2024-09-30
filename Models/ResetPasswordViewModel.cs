using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class ResetPasswordViewModel
    {
        // O atributo [Required] indica que o campo é obrigatório e deve ser preenchido.
        // Se o campo estiver vazio, uma mensagem de erro será gerada.
        [Required(ErrorMessage = "O nome de utilizador é obrigatório.")]
        public string UserName { get; set; }

        // O atributo [Required] indica que o campo é obrigatório e deve ser preenchido.
        // O atributo [DataType(DataType.Password)] configura o campo para ser tratado como uma senha,
        // ocultando o texto inserido no campo.
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // O atributo [Required] indica que o campo é obrigatório e deve ser preenchido.
        // O atributo [DataType(DataType.Password)] configura o campo para ser tratado como uma senha.
        // O atributo [Compare("Password")] valida se o valor inserido no campo ConfirmPassword
        // corresponde ao valor da senha principal (Password).
        [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmPassword { get; set; }

        // O atributo [Required] indica que o campo é obrigatório e deve ser preenchido.
        // Este campo armazena o token de redefinição de senha, que é necessário para validar o pedido de redefinição.
        [Required(ErrorMessage = "O token é obrigatório.")]
        public string Token { get; set; }
    }
}
