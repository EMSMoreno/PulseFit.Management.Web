namespace PulseFit.Management.Web.Data.Entities
{
    public class Workout : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; } // Em minutos

        public string Type { get; set; } // Ex: Cardio, Strength, Yoga

        public int Popularity { get; set; }

        public int DifficultyLevel { get; set; } // Ex: 1 a 5

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int MaxCapacity { get; set; }

        public string Status { get; set; }

        public int InstructorId { get; set; }

        public User Instructor { get; set; } // Relação com o instrutor (User)

        public int GymId { get; set; }

        public Gym Gym { get; set; }

    }
}
