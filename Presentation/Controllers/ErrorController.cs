using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class ErrorController : Controller
{
    [Route("404")]
    public IActionResult NotFoundPage()
    {
        return View();
    }

    [Route("500")]
    public IActionResult ServerError()
    {
        return View();
    }
}