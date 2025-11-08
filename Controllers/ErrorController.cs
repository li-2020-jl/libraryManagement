// Controllers/ErrorController.cs
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace LibraryManagement.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult Index(int statusCode)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            ViewBag.OriginalPath = feature?.OriginalPath;
            ViewBag.OriginalQuery = feature?.OriginalQueryString;
            ViewBag.StatusCode = statusCode;
            return View("Error");
        }

        [HttpGet]
        [Route("Error/Exception")]
        public IActionResult Exception()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var exception = feature?.Error;

            string message = "Something went wrong.";

            switch (exception)
            {
                case FileNotFoundException:
                    message = "The file was not found.";
                    break;
                case UnauthorizedAccessException:
                    message = "You do not have permission.";
                    break;
                case NullReferenceException:
                    message = "A system error occurred.";
                    break;
            }

            ViewBag.ErrorMessage = message;
            ViewBag.Path = feature?.Path;

            return View("Exception");
        }
    }
}