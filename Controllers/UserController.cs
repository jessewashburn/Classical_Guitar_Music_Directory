using CGMD.Models;
using Microsoft.AspNetCore.Mvc;

namespace CGMD.Controllers
{
    /// <summary>
    /// Controller responsible for handling user-related actions.
    /// </summary>
    public class UserController : Controller
    {
        /// <summary>
        /// Handles the creation of a new user.
        /// </summary>
        /// <param name="user">The user object to be created.</param>
        /// <returns>
        /// Redirects to the index action of the Home controller if the user is successfully created.
        /// Returns to the same view with validation errors if the model state is invalid.
        /// </returns>
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
