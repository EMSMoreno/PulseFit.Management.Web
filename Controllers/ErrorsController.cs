using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Models;

namespace PulseFit.Management.Web.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier
            };

            switch (statusCode)
            {
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

            return View("InternalError", errorViewModel);
        }
    }
}
