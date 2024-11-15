using System;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models.Validations
{
    public class BirthdateValidationAttribute : ValidationAttribute
    {
        private readonly DateTime _minDate;
        private readonly int _minimumAge;
        private readonly int _maximumAge;

        // Construtor que aceita uma idade mínima e máxima como parâmetros
        public BirthdateValidationAttribute(int minimumAge = 16, int maximumAge = 100, string minDate = "1920-01-01")
        {
            _minimumAge = minimumAge;
            _maximumAge = maximumAge;
            _minDate = DateTime.Parse(minDate);
        }

        public override bool IsValid(object value)
        {
            if (value is DateTime birthdate)
            {
                DateTime maxDate = DateTime.Today.AddYears(-_minimumAge);
                DateTime minValidDate = DateTime.Today.AddYears(-_maximumAge);

                return birthdate >= _minDate && birthdate <= maxDate && birthdate >= minValidDate;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            DateTime maxDate = DateTime.Today.AddYears(-_minimumAge);
            DateTime minValidDate = DateTime.Today.AddYears(-_maximumAge);
            return $"The {name} field must be a valid date between {minValidDate:yyyy-MM-dd} and {maxDate:yyyy-MM-dd} (age must be between {_minimumAge} and {_maximumAge} years).";
        }
    }
}
