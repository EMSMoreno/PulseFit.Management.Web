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

        //Esta action vai ser executada quando entrar por este caminho error/404
        [Route("error/404")]
        //Este action é só para mostrar a página do Error404
        //Depois de elaborado este metodo temos que criar a respectiva View para o Error404 para isso
        //clicamos com o botao direito sobre Error404() e fazemos Add View - Razor View - Add
        public IActionResult Error404()
        {
            return View();
        }
    }
}
