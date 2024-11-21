using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Models;
using System.Diagnostics;
 
namespace PulseFit.Management.Web.Controllers
{
    public class ErrorsController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // This action will be executed when you enter this path error/404
        [Route("error/404")]
        //This action is just to show the Error404 page
        //After creating this method, we have to create the respective View for Error404 for this
        //right click on Error404() and do Add View - Razor View - Add
        public IActionResult Error404()
        {
            return View();
        }
    }
}
