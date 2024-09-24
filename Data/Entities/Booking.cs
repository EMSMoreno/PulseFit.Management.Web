namespace PulseFit.Management.Web.Data.Entities
{
    public class Booking : IEntity
    {
        public int Id { get; set; }

        public DateTime ReservationDate { get; set; }

        public string Status { get; set; } // Ex: Confirmed, Canceled

        public int WorkoutId { get; set; }

        public Workout Workout { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime TrainingDate { get; set; }

        public int GymId { get; set; }

        public Gym Gym { get; set; }
    }
}
