using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class LoginViewModel
    {
        //O Username é obrigatorio
        [Required]
        //O Username será um email
        [EmailAddress]
        public string Username { get; set; }

        //A Password será obrigatoria
        [Required]
        //A Password terá que ter no minimo 6 caracteres
        [MinLength(6)]
        public string Password { get; set; }

        //Esta propriedade vai servir para a pessoa nao precisar de estar sempre a fazer o Login
        public bool RememberMe { get; set; }
    }
}