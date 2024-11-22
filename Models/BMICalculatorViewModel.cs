using System.ComponentModel.DataAnnotations;

public class BMICalculatorViewModel
{
    [Required(ErrorMessage = "Height is required")]
    [Range(1, 300, ErrorMessage = "Height must be between 1 and 300 cm")]
    public double? Height { get; set; } // Altura em cm

    [Required(ErrorMessage = "Weight is required")]
    [Range(1, 500, ErrorMessage = "Weight must be between 1 and 500 kg")]
    public double? Weight { get; set; } // Peso em kg

    [Required(ErrorMessage = "Age is required")]
    [Range(1, 120, ErrorMessage = "Age must be between 1 and 120 years")]
    public int? Age { get; set; } // Idade em anos

    [Required(ErrorMessage = "Sex is required")]
    public Sex? Sex { get; set; } // Masculino ou Feminino

    public double? BMI { get; set; } // Calculado no backend
    public string? Status { get; set; } // Status baseado no BMI
}

public enum Sex
{
    Male,
    Female
}
