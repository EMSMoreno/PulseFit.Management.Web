using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class PersonalTrainer : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Specialty { get; set; }

        public string Certification { get; set; }

        public List<Client> Clients { get; set; } // Lista de clientes

        public DateTime HireDate { get; set; }

        public string Status { get; set; }

        // Identificador do utilizador r. É obrigatório e serve como chave estrangeira.
        [Required]
        public string UserId { get; set; }

        // Navegação para a entidade `User`. Representa o utilizador.
        public User User { get; set; }
    }
}
