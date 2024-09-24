using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Employee : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; } // Ex: Secretary, Admin

        public DateTime HireDate { get; set; }

        public string Status { get; set; }

        public string PhoneNumber { get; set; }

        public string Shift { get; set; } // Turno de trabalho

        // Identificador do User. É obrigatório e serve como chave estrangeira.
        [Required]
        public string UserId { get; set; }

        // Navegação para a entidade `User`. Representa o utilizador.
        public User User { get; set; }
    }
}
