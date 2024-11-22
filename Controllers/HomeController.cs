using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PulseFit.Management.Web.Models;
using System.Diagnostics;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AboutUs()
    {
        return View();
    }

    public IActionResult Bmi()
    {
        return View(new BMICalculatorViewModel());
    }

    public IActionResult Services()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult BmiCalculator(BMICalculatorViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Calcula o IMC
            model.BMI = model.Weight / ((model.Height / 100) * (model.Height / 100));

            // Determina o status baseado no IMC
            if (model.Age < 18)
            {
                model.Status = "Consult a pediatrician for BMI evaluation.";
            }
            else if (model.Sex == Sex.Male)
            {
                if (model.BMI < 18.5) model.Status = "Underweight";
                else if (model.BMI < 24.9) model.Status = "Healthy";
                else if (model.BMI < 29.9) model.Status = "Overweight";
                else model.Status = "Obese";
            }
            else if (model.Sex == Sex.Female)
            {
                if (model.BMI < 18.0) model.Status = "Underweight";
                else if (model.BMI < 24.0) model.Status = "Healthy";
                else if (model.BMI < 29.0) model.Status = "Overweight";
                else model.Status = "Obese";
            }

            // Log para verificar os valores calculados
            _logger.LogInformation($"BMI Calculation: Height = {model.Height}, Weight = {model.Weight}, BMI = {model.BMI}, Status = {model.Status}");
        }
        else
        {
            // Log para saber porque o modelo não é válido
            var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                      .Select(e => e.ErrorMessage)
                                                      .ToList();
            _logger.LogWarning($"ModelState is invalid. Validation errors: {string.Join(", ", validationErrors)}");
        }

        return View("Bmi", model);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
