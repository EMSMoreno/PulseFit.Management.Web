using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Client : IEntity
    { 
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime Birthdate { get; set; }

        public string Address { get; set; }

        public int SubscriptionPlanId { get; set; }

        public Subscription SubscriptionPlan { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string Status { get; set; }

        public string ClientImagePath { get; set; } // Caminho para a imagem do cliente

        // Identificador do utilizador. É obrigatório e serve como chave estrangeira.
        [Required]
        public string UserId { get; set; }

        // Navegação para a entidade `User`. Representa o utilizador.
        public User User { get; set; }
    }
}
