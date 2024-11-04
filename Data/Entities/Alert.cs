namespace PulseFit.Management.Web.Data.Entities
{
    public class Alert : IEntity
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsResolved { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } // Relationship with the User Entity
    }
}
