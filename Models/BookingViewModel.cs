using static PulseFit.Management.Web.Data.Entities.Booking;

namespace PulseFit.Management.Web.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }

        public DateTime ReservationDate { get; set; }

        public BookingStatus Status { get; set; }

        public int WorkoutId { get; set; }

        public string? WorkoutName { get; set; }

        public string? UserName { get; set; }

        public string UserId { get; set; }

        public DateTime TrainingDate { get; set; }

        public string? GymName { get; set; }

        public int GymId { get; set; }
    }
}
