namespace PulseFit.Management.Web.Data.Entities
{
    public class NutritionPlan : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public int Duration { get; set; } // Duração em dias

        public int UserId { get; set; }

        public User User { get; set; }

        public int GymId { get; set; }

        public Gym Gym { get; set; }

        public string Status { get; set; }

        public int ClientId { get; set; }
    }
}
