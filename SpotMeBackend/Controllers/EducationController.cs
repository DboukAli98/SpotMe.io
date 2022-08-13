using Microsoft.AspNetCore.Mvc;

namespace SpotMeBackend.Controllers;

public class EducationController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}