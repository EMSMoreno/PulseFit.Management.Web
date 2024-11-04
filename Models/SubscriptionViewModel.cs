using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Models
{
    public class SubscriptionViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public decimal Price { get; set; }

        public int MaxWorkouts { get; set; }

        public int DurationMonths { get; set; }

        public SubscriptionStatus Status { get; set; }

        public Guid ImageId { get; set; }

        [Display(Name = "Subscription Image")]
        public IFormFile? SubscriptionImageFile { get; set; }

        public string ImageUrl => ImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/subscription-images/{ImageId}.jpg";
    }
}
