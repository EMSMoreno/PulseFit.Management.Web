using System.ComponentModel.DataAnnotations;

public class BMICalculatorViewModel
{
    // Height in centimeters, required field with a valid range
    [Required(ErrorMessage = "Height is required")]
    [Range(1, 300, ErrorMessage = "Height must be between 1 and 300 cm")]
    public double? Height { get; set; }

    // Weight in kilograms, required field with a valid range
    [Required(ErrorMessage = "Weight is required")]
    [Range(1, 500, ErrorMessage = "Weight must be between 1 and 500 kg")]
    public double? Weight { get; set; }

    // Age in years, required field with a valid range
    [Required(ErrorMessage = "Age is required")]
    [Range(1, 120, ErrorMessage = "Age must be between 1 and 120 years")]
    public int? Age { get; set; }

    // Sex (Male or Female), required field
    [Required(ErrorMessage = "Sex is required")]
    public Sex? Sex { get; set; }

    // Calculated BMI (to be set in the backend)
    public double? BMI { get; set; }

    // Status based on the BMI value (e.g., Underweight, Normal, Overweight, Obese)
    public string? Status { get; set; }
}

// Enumeration for Sex
public enum Sex
{
    Male,
    Female
}
