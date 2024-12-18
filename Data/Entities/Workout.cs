﻿using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Workout : IEntity
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public WorkoutType Type { get; set; }

        public enum WorkoutType
        {
            Individual,
            Group
        }

        public IndividualWorkoutType? IndividualType { get; set; }

        public enum IndividualWorkoutType
        {
            Strenght,
            Hypertrophy,
            Calisthenics,
            Functional,
            Cardio,
            Crossfit,
            Rehabilitation,
            Prenatal,
            Postnatal,
            Senior,
        }

        public GroupWorkoutType? GroupType { get; set; }

        public enum GroupWorkoutType
        {
            Yoga,
            Pilates,
            HIIT,
            Martial_Arts,
            Dance,
            Swimming,
            Boxing,
            KickBoxing,
            Mobility,
            Meditation,
            Cycling,
            Aqua_Aerobics
        }

        public int? Popularity { get; set; }

        [Required]
        public WorkoutDifficulty DifficultyLevel { get; set; }

        public enum WorkoutDifficulty
        {
            Easy,
            Medium,
            Hard
        }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int MaxCapacity { get; set; }

        [Required]
        public WorkoutStatus Status { get; set; }

        public enum WorkoutStatus
        {
            Scheduled,
            Ongoing,
            Cancelled,
            Finished
        }

        [Required]
        public string InstructorId { get; set; }

        public string? InstructorName { get; set; }

        [Required]
        public int GymId { get; set; }

        public string? GymName { get; set; }

        [Required]
        public int Bookings { get; set; }

        [Display(Name = "Workout Image")]
        public Guid WorkoutImageId { get; set; }

        public string WorkoutImageUrl => WorkoutImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/workouts-pics/{WorkoutImageId}.jpg";
    }
}
