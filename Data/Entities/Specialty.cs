using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Specialty : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; } // URL completa da imagem

        public string? ImageName { get; set; } // Nome do arquivo da imagem, caso necessário

        public List<PersonalTrainer> PersonalTrainers { get; set; } = new List<PersonalTrainer>();
    }
}
