using System;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Employee : IEntity
    {
        public int Id { get; set; }

        [Required]
        public EmployeeType EmployeeType { get; set; } // Defines the Type of Employee

        public DateTime? HireDate { get; set; } // Optional Hire Date

        public Status Status { get; set; } = Status.Active;

        public ShiftType Shift { get; set; } // Use enum ShiftType

        [Required]
        public string UserId { get; set; } // FK for User

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
