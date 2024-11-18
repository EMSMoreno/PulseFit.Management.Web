using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Gym : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public int Capacity { get; set; }

        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public DateTime CreationDate { get; set; }

        public GymStatus Status { get; set; }

        public enum GymStatus
        {
            Active,
            Inactive,
        }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public GymDayOff DayOff { get; set; } 

        public enum GymDayOff
        {
            None,
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
        }

        [Display(Name="Gym Image")]
        public Guid GymImageId { get; set; }

        public string GymImageUrl => GymImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/gyms-pics/{GymImageId}.png";
    }
}