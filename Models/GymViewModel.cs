using System.ComponentModel.DataAnnotations;
using static PulseFit.Management.Web.Data.Entities.Gym;

namespace PulseFit.Management.Web.Models
{
    public class GymViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public int Capacity { get; set; }

        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public DateTime CreationDate { get; set; }

        public GymStatus Status { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public GymDayOff DayOff { get; set; }

        [Display(Name = "Gym Picture")]
        public IFormFile? GymImageFile { get; set; }
    }
}
