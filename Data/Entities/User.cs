using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [Display(Name = "Profile Picture")]
        public Guid? ProfilePictureId { get; set; }

        public string ProfilePictureUrl => ProfilePictureId == null
            ? "/images/noimage.png"
            : $"/uploads/profile-pics/{ProfilePictureId}.jpg";

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [Display(Name = "Last Login")]
        public DateTime? LastLogin { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
