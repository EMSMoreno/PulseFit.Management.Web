namespace PulseFit.Management.Web.Data.Entities
{
    public class Membership
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; } // Null if subscription is active

        public bool IsPendingFee { get; set; } // Indicates if there are outstanding fees
    }
}
