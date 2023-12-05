using Microsoft.AspNetCore.Mvc;

namespace CGMD.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
