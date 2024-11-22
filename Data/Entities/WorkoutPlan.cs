using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class WorkoutPlan : IEntity
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

        public WorkoutPlanTypeList WorkoutPlanType { get; set; }

        public enum WorkoutPlanTypeList
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

        public List<Equipment> Equipments { get; set; } = new List<Equipment>();

        [Display(Name = "Workout Image")]
        public Guid WorkoutPlanImageId { get; set; }

        public string WorkoutPlanImageUrl => WorkoutPlanImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/workoutsPlans-pics/{WorkoutPlanImageId}.jpg";
    }
}