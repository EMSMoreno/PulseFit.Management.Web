using Microsoft.AspNetCore.Identity;

namespace PulseFit.Management.Web.Data.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        public string UserType { get; set; } // Ex: Admin, Cliente, Funcionário, Nutritionist, Personal Trainer

        public string Address { get; set; }

        public string Phone { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string Status { get; set; } // Ex: Active, Inactive

        public string ClientProfilePicPath { get; set; } // Image Path
    }
}
