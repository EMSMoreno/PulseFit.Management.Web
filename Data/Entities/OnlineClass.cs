using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class OnlineClass : IEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string VideoUrl { get; set; }

        public ClassCategory Category { get; set; }

        public enum ClassCategory
        {
            Yoga,
            Pilates,
            Dance,
            Mobility,
            Meditation,
            HIIT
        }

        [Display(Name = "Class Image")]
        public Guid? ClassImageId { get; set; }

        public string ClassImageUrl => ClassImageId == null
           ? "/images/noimage.png"
           : $"/uploads/onlineClasses-pics/{ClassImageId}.jpg";
    }
}
