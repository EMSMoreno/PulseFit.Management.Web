namespace PulseFit.Management.Web.Data.Entities
{
    public class Workout : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; } 

        public WorkoutType Type { get; set; }

        public enum WorkoutType
        {
            Cardio,
            Strenght,
            Yoga,
            Cycling,
            Bumbum
        }

        public int Popularity { get; set; }

        public WorkoutDifficulty DifficultyLevel { get; set; } 

        public enum WorkoutDifficulty
        {
            Easy,
            Medium,
            Hard
        }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int MaxCapacity { get; set; }

        public WorkoutStatus Status { get; set; }

        public enum WorkoutStatus
        {
            Scheduled,
            Ongoing,
            Cancelled,
            Finished
        }

        public string InstructorId { get; set; } 

        public User Instructor { get; set; }

        public int GymId { get; set; }

        public Gym Gym { get; set; }

        public int Bookings { get; set; }
    }
}
