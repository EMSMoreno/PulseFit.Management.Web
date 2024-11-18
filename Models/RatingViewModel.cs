namespace PulseFit.Management.Web.Models
{
    public class RatingViewModel
    {
        public int WorkoutId { get; set; }

        public int RatingValue { get; set; } // 1 to 5 stars

        public string Comment { get; set; }
    }
}
