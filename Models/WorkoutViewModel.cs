using PulseFit.Management.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class WorkoutViewModel : Workout
    {
        [Display(Name = "Workout Picture")]
        public IFormFile? WorkoutImageFile { get; set; }
    }
}
