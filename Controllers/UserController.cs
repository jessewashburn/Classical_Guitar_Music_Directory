using CGMD.Models;
using Microsoft.AspNetCore.Mvc;

namespace CGMD.Controllers
{
    public class UserController : Controller
    {

        [HttpPost]
        public IActionResult Create(User user)
        {
            // Check if the model state is valid for the User object
            if (ModelState.IsValid)
            {
                // Your logic for creating a user, such as saving to a database
                // ...

                // Redirect to the index action or any other appropriate action
                return RedirectToAction("Index", "Home"); // Replace "Home" with the appropriate controller
            }

            // If the model state is not valid, return to the form with validation errors
            return View(user);
        }
    }

}
