using Microsoft.AspNetCore.Mvc;

namespace Pustok_BackEndProject.Controllers;

public class HomeController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }


}
