using CGMD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CGMD.Controllers
{
    /// <summary>
    /// Controller responsible for handling requests to the home-related views.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the HomeController class.
        /// </summary>
        /// <param name="logger">Logger for capturing runtime logs.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns the main homepage view.
        /// </summary>
        /// <returns>The Index view.</returns>
        public IActionResult Index()
        {
            return View("Home");
        }

        /// <summary>
        /// Returns the Privacy policy view.
        /// </summary>
        /// <returns>The Privacy view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Returns the About page view.
        /// </summary>
        /// <returns>The About view.</returns>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Returns the Contact page view.
        /// </summary>
        /// <returns>The Contact view.</returns>
        public IActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// Handles errors and returns the Error view.
        /// This action is configured not to cache the response.
        /// </summary>
        /// <returns>The Error view with error details.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
