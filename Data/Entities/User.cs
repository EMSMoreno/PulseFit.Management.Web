using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    // A classe `User` herda de `IdentityUser`, que é uma implementação padrão do ASP.NET Core Identity para a entidade de utilizador.
    public class User : IdentityUser
    {
        // Nome próprio do utilizador. O comprimento máximo é de 50 caracteres.
        [MaxLength(50, ErrorMessage = "O campo {0} só pode conter {1} caracteres")]
        public string FirstName { get; set; }

        // Apelido do utilizador. O comprimento máximo é de 50 caracteres.
        [MaxLength(50, ErrorMessage = "O campo {0} só pode conter {1} caracteres")]
        public string LastName { get; set; }

        // Endereço do utilizador. O comprimento máximo é de 100 caracteres.
        [MaxLength(100, ErrorMessage = "O campo {0} só pode conter {1} caracteres")]
        public string? Address { get; set; }

        // Identificador da imagem de perfil do utilizador. É do tipo `Guid` e representa a imagem de perfil do utilizador.
        [Display(Name = "Profile Picture")]
        public Guid ProfilePictureId { get; set; }

        // Nome completo do utilizador, composto pelo nome próprio e apelido.
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        // Data em que o utilizador foi criado. Esta data pode ser usada para registar quando o utilizador foi criado na aplicação.
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        // Data do último login do utilizador. Esta data pode ser usada para registar quando o utilizador fez o último login.
        [Display(Name = "Last Login")]
        public DateTime LastLogin { get; set; }
    }
}
