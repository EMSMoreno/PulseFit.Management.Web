namespace PulseFit.Management.Web.Data.Entities
{
    public class Equipment : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; } // Ex: Cardio, Strength

        public int Quantity { get; set; }

        public string Status { get; set; }

        public int GymId { get; set; }

        public Gym Gym { get; set; }
    }
}
