using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Workout : IEntity
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public int Duration { get; set; }

        [Required]
        public WorkoutType Type { get; set; }

        public enum WorkoutType
        {
            Cardio,
            Strenght,
            Yoga,
            Cycling,
            Bumbum
        }

        [Required]
        public int Popularity { get; set; }

        [Required]
        public WorkoutDifficulty DifficultyLevel { get; set; } 

        public enum WorkoutDifficulty
        {
            Easy,
            Medium,
            Hard
        }

        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public int MaxCapacity { get; set; }
        
        [Required]
        public WorkoutStatus Status { get; set; }

        public enum WorkoutStatus
        {
            Scheduled,
            Ongoing,
            Cancelled,
            Finished
        }

        [Required]
        public string InstructorId { get; set; } 

        public string? InstructorName { get; set; }

        [Required]
        public int GymId { get; set; }

        public string? GymName { get; set; }

        [Required]
        public int Bookings { get; set; }
    }
}
