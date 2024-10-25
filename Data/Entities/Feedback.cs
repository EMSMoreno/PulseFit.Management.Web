namespace PulseFit.Management.Web.Data.Entities
{
    public class Feedback : IEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Tipo alterado para string
        public User User { get; set; }
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }
        public int GymId { get; set; }
        public Gym Gym { get; set; }
        public int Rating { get; set; } // Ex: 1 a 5
        public string Comment { get; set; }
        public DateTime FeedbackDate { get; set; }
    }
}
