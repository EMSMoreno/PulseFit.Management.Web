using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class ChangeUserViewModel
    {
        //Vai ser obrigatorio preencher o FirstName
        [Required]
        //Este Display faz com que depois o nome apareça separado 
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //Vai ser obrigatorio preencher o LastName
        [Required]
        //Este Display faz com que depois o nome apareça separado 
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Campo para o endereço do utilizador com comprimento máximo de 100 caracteres.
        // O atributo [MaxLength] define a mensagem de erro se o limite for excedido.
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length")]
        public string Address { get; set; }

        // Campo para o número de telefone do utilizador com comprimento máximo de 20 caracteres.
        // O atributo [MaxLength] define a mensagem de erro se o limite for excedido.
        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters length")]
        public string PhoneNumber { get; set; }
    }
}