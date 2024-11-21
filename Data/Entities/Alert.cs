using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Alert : IEntity
    {
        public int Id { get; set; }


        [MaxLength(500)]
        public string Message { get; set; }


        public DateTime CreatedAt { get; set; }

        // Indicates whether the alert has been resolved or not
        public bool IsResolved { get; set; }


        public int EmployeeId { get; set; }

        // Navigation to the Employee entity
        public Employee Employee { get; set; }
    }
}
