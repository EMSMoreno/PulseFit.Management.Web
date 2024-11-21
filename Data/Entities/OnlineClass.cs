
namespace PulseFit.Management.Web.Data.Entities
{
    public class OnlineClass : IEntity
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? ThumbnailUrl { get; set; }

        public string VideoUrl { get; set; }

        public ClassCategory Category { get; set; }

        public enum ClassCategory
        {
            Yoga,
            Pilates
        }
    }
}
