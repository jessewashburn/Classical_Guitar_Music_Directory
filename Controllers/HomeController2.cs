using Microsoft.AspNetCore.Mvc;

namespace CGMD.Controllers
{
    public class HomeController2 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
