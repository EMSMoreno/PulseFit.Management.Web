namespace PulseFit.Management.Web.Data.Entities
{
    public class Equipment : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public EquipmentType Type { get; set; }

        public enum EquipmentType
        {
            Cardio,
            Chest,
            Biceps,
            Triceps,
            Shoulders,
            Lats,
            Quadriceps,
            Hamstrings,
            Calves,
            Glutes,
            Abs,
            Forearms
        }

        public EquipmentStatus Status { get; set; }

        public enum EquipmentStatus
        {
            Active,
            Inactive
        }

        public int GymId { get; set; }

        public string? GymName { get; set; }

        public Guid? EquipmentImageId { get; set; }

        public string EquipmentImageUrl => EquipmentImageId == null
            ? "/images/noimage.png"
            : $"/uploads/equipments-pics/{EquipmentImageId}.jpg";

        public List<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
    }
}
