using Microsoft.AspNetCore.Mvc;
using Project_Manager.Models;

namespace Project_Manager.Controllers
{
    public class UserController : Controller
    {
        public IActionResult SignIn()
        {
            var db = new project_manager_dbContext();

            var model = db.User.ToList();

            return View(model);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(User formData)
        {
            //TODO check if all fields were filled
            //TODO check if the user already exist
            //TODO show errors to user
            //TODO show success
            var user = new User();
            var db = new project_manager_dbContext();

            user.Name = formData.Name;
            user.Email = formData.Email;
            user.Password = formData.Password;

            db.User.Add(user);
            db.SaveChanges();

            return RedirectToAction("SignIn");
            //TODO redirect to signup and show success
        }
    }
}
