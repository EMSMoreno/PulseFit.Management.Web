using System.ComponentModel.DataAnnotations;
 
namespace PulseFit.Management.Web.Models
{
    public class ChangePasswordViewModel
    {
        //Vai ser obrigatorio preencher a Password antiga
        [Required]
        [Display(Name = "Current password")]
        //Aqui vai por a Password antiga
        public string OldPassword { get; set; }

        //Vai ser obrigatorio preencher a Password nova
        [Required]
        [Display(Name = "New password")]
        //Aqui vai por a Password nova
        public string NewPassword { get; set; }

        //Confirmaçao, que serve para o utilizador por a password 2 vexes
        [Required]
        //Aqui vai confirmar que o valor que o utilizador poem no segundo campo é igual ao valor que defeniu para a New password
        [Compare("NewPassword")]
        public string Confirm { get; set; }
    }
}