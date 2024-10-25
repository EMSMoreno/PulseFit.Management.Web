namespace PulseFit.Management.Web.Data.Entities
{
    public class Alert : IEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Tipo alterado para string
        public User User { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsResolved { get; set; }
        public int GymId { get; set; }
        public Gym Gym { get; set; }
    }
}
