using System.ComponentModel.DataAnnotations;
using static PulseFit.Management.Web.Data.Entities.Workout;

namespace PulseFit.Management.Web.Models
{
    public class WorkoutViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public WorkoutType Type { get; set; }

        public IndividualWorkoutType? IndividualType { get; set; }

        public GroupWorkoutType? GroupType { get; set; }

        public int Popularity { get; set; }

        public WorkoutDifficulty DifficultyLevel { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int MaxCapacity { get; set; }

        public WorkoutStatus Status { get; set; }

        public string InstructorId { get; set; }

        public string? InstructorName { get; set; }

        public int GymId { get; set; }

        public string? GymName { get; set; }

        public int Bookings { get; set; }

        [Display(Name= "Workout Picture")]
        public IFormFile? WorkoutImageFile { get; set; }
    }
}
