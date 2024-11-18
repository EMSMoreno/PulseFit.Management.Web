namespace PulseFit.Management.Web.Models
{
    public class WorkoutPlanViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public WorkoutPlanDifficulty Difficulty { get; set; }

        public enum WorkoutPlanDifficulty
        {
            Easy,
            Medium,
            Hard
        }

        public WorkoutType WorkoutPlanType { get; set; }

        public enum WorkoutType
        {
            FullBody,
            Upper,
            Lower,
            Push,
            Pull,
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

        public List<int> EquipmentIds { get; set; } = new List<int>();

        public List<EquipmentItemViewModel> Equipments { get; set; } = new List<EquipmentItemViewModel>();

        public Guid WorkoutPlanImageId { get; set; }

        public string WorkoutPlanImageUrl => WorkoutPlanImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/workoutsPlans-pics/{WorkoutPlanImageId}.jpg";

        public IFormFile? WorkoutPlanImageFile { get; set; }
    }
}
