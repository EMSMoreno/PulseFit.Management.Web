namespace PulseFit.Management.Web.Data.Entities
{
    public class NutritionPlan : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int Duration { get; set; } // Duration in Days
        public string UserId { get; set; } // Changed to String
        public User User { get; set; }
        public int GymId { get; set; }
        public Gym Gym { get; set; }
        public string Status { get; set; }
        public string ClientId { get; set; } // Changed to String
    }
}
