using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Report : IEntity
    {
        public int Id { get; set; }

        public string ReportType { get; set; } // Ex: Financial, Usage

        public DateTime GenerationDate { get; set; }

        public string Description { get; set; }

        public string Data { get; set; } // Stores report content

        // User identifier. It is mandatory and serves as a foreign key.
        [Required]
        public string UserId { get; set; }

        // Navigation to the `User` entity. Represents the user.
        public User User { get; set; }
    }
}
