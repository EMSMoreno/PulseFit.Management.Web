using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PulseFit.Management.Web.Models;

public class ErrorsController : Controller
{
    private readonly ILogger<ErrorsController> _logger;

    public ErrorsController(ILogger<ErrorsController> logger)
    {
        _logger = logger;
    }

    [Route("Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        _logger.LogWarning($"HTTP Status Code: {statusCode}, Request ID: {HttpContext.TraceIdentifier}");
        var errorViewModel = new ErrorViewModel
        {
            RequestId = HttpContext.TraceIdentifier
        };

        switch (statusCode)
        {
            case 401:
                errorViewModel.ErrorMessage = "You are not authorized to view this page.";
                return View("AccessDenied", errorViewModel);
            case 403:
                errorViewModel.ErrorMessage = "You do not have permission to access this page.";
                return View("AccessDenied", errorViewModel);
            case 404:
                errorViewModel.ErrorMessage = "The page you are looking for could not be found.";
                return View("NotFound", errorViewModel);
            default:
                errorViewModel.ErrorMessage = "An unexpected error occurred.";
                return View("Error", errorViewModel);
        }
    }

    [Route("Error")]
    public IActionResult Error()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var errorViewModel = new ErrorViewModel
        {
            RequestId = HttpContext.TraceIdentifier,
            ErrorMessage = exceptionHandlerPathFeature?.Error.Message,
            StackTrace = exceptionHandlerPathFeature?.Error.StackTrace
        };

        _logger.LogError(exceptionHandlerPathFeature?.Error, "An unexpected error occurred.");

        return View("InternalError", errorViewModel);
    }
}
