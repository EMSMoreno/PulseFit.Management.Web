namespace PulseFit.Management.Web.Data.Entities
{
    public class Equipment : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // Equipment Type: Cardio, Bodybuilding and Others
        public EquipmentType Type { get; set; }

        public int Quantity { get; set; }

        // Equipment Status: Active or Inactive
        public EquipmentStatus Status { get; set; }

        public int GymId { get; set; }

        public Gym Gym { get; set; }
    }

    // Enum for Equipment Type
    public enum EquipmentType
    {
        Cardio,
        Bodybuilding,
        Others
    }

    // Enum for Equipment Status
    public enum EquipmentStatus
    {
        Active,
        Inactive
    }
}
