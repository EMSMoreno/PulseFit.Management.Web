namespace PulseFit.Management.Web.Data.Entities
{
    public class Gym : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public int Capacity { get; set; }

        public DateTime OpeningTime { get; set; }

        public DateTime ClosingTime { get; set; }

        public DateTime CreationDate { get; set; }

        public string Status { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string DayOff { get; set; } // Dia de folga

        public string GymImagePath { get; set; } // Caminho para a imagem do ginásio
    }
}
