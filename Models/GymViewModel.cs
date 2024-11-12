using PulseFit.Management.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class GymViewModel : Gym
    {
        [Display(Name = "Gym Picture")]
        public IFormFile? GymImageFile { get; set; }
    }
}
