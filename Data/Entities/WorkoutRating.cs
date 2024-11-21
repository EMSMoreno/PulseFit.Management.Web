namespace PulseFit.Management.Web.Data.Entities
{
    public class WorkoutRating
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } // Relationship with the User entity
        public int WorkoutId { get; set; } // Workout being evaluated
        public int RatingValue { get; set; } // Rating value (1 to 5 stars)
        public string Comment { get; set; } // Opcional Comment
        public DateTime DateCreated { get; set; }
    }
}
