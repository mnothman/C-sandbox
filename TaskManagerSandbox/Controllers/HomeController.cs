using Microsoft.AspNetCore.Mvc;

namespace TaskManagerSandbox.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult Index()
    {
        return File("~/index.html", "text/html");
    }

    [HttpGet("/app")]
    public IActionResult App()
    {
        return File("~/index.html", "text/html");
    }
} 