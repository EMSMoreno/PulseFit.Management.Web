using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Nutritionist : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Specialization { get; set; }

        public int ExperienceYears { get; set; }

        // Identificador do utilizador r. É obrigatório e serve como chave estrangeira.
        [Required]
        public string UserId { get; set; }

        // Navegação para a entidade `User`. Representa o utilizador.
        public User User { get; set; }
    }
}
