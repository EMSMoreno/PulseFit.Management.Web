namespace PulseFit.Management.Web.Data.Entities
{
    public class AdminLog
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public string Type { get; set; } // Ex: Error, Info

        public DateTime CreatedAt { get; set; }
    }
}
