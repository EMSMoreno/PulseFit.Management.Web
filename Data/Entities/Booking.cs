namespace PulseFit.Management.Web.Data.Entities
{
    public class Booking : IEntity
    {
        public int Id { get; set; }

        public DateTime ReservationDate { get; set; }

        public BookingStatus Status { get; set; } 

        public enum BookingStatus
        {
            Reserved,
            Confirmed,
            Canceled
        }

        public int WorkoutId { get; set; }

        public string? UserName { get; set; }

        public string UserId { get; set; } 

        public DateTime TrainingDate { get; set; }

        public string? GymName { get; set; }

        public int GymId { get; set; }
    }
}
