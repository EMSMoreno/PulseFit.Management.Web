using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc;


namespace PulseFit.Management.Web.Helpers
{
    public class NotFoundViewResult : ViewResult
    {
        public NotFoundViewResult(string viewName)
        {
            ViewName = viewName;
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
