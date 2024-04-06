using Microsoft.AspNetCore.Mvc;
using Pustok_BackEndProject.Contexts;

namespace Pustok_BackEndProject.Areas.Admin.Controllers;
[Area("Admin")]
public class DashboardController : Controller
{

    public IActionResult Index()
    {
        return View();
    }
}
