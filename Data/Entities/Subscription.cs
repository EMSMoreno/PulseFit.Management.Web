using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Subscription : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int MaxWorkouts { get; set; } // Máximo de treinos permitidos

        public int Duration { get; set; } // Duração em meses

        public DateTime CreationDate { get; set; }

    }
}
