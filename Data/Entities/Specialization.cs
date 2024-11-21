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

        public string? ImageUrl { get; set; }  // URL to the image, such as a reference photo for specialization

        public string? ImageName { get; set; } // Image file name if necessary

        // List of nutritionists associated with this specialization
        public List<Nutritionist> Nutritionists { get; set; } = new List<Nutritionist>();
    }
}
