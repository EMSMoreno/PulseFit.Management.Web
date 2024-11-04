using System;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Employee : IEntity
    {
        public int Id { get; set; }

        [Required]
        public EmployeeType EmployeeType { get; set; } // Define o tipo de funcionário

        public DateTime? HireDate { get; set; } // Data de contratação opcional

        public Status Status { get; set; } = Status.Active;

        public ShiftType Shift { get; set; } // Usa o enum ShiftType

        [Required]
        public string UserId { get; set; } // FK para User

        public User User { get; set; }
    }

    public enum EmployeeType
    {
        Secretary,
        CleaningStaff,
        Receptionist,
        Manager,
        Other
    }

    public enum ShiftType
    {
        Morning,
        Afternoon,
        Evening,
        Night,
        Flexible,
        None
    }
}
