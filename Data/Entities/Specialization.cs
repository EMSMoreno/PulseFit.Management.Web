using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Specialization : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }  // URL para a imagem, como uma foto de referência para a especialização

        public string? ImageName { get; set; } // Nome do arquivo da imagem, se necessário

        // Lista de nutricionistas associados a esta especialização
        public List<Nutritionist> Nutritionists { get; set; } = new List<Nutritionist>();
    }
}
