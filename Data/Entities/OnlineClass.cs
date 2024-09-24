namespace PulseFit.Management.Web.Data.Entities
{
    public class OnlineClass : IEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string VideoUrl { get; set; } // URL do vídeo

        public int Duration { get; set; } // Em minutos

        public string Category { get; set; } // Ex: Cardio, Strength, Yoga

        public int InstructorId { get; set; }

        public User Instructor { get; set; }

        public int Rating { get; set; } // Ex: 1 a 5
    }
}
