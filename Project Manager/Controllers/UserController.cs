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
        public IActionResult SignInUser(User formData)
        {
            var db = new project_manager_dbContext();

            var user = db.User.ToList();

            var findUser = db.User.Where(u => u.Email == formData.Email && u.Password == formData.Password).ToList();

            Console.WriteLine(findUser.Count());

            if (findUser.Count() == 1)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = "Something Went Wrong Please Try Again";
            return RedirectToAction("SignIn");

        }

        [HttpPost]
        public IActionResult SignUpUser(User formData)
        {
            //TODO check if all fields were filled
            //TODO redirect to signup and show errors
            //TODO redirect to signup and show success
            var db = new project_manager_dbContext();

            var findUser = db.User.Where(u => u.Email == formData.Email).ToList();

            Console.WriteLine(findUser.Count());

            if (findUser.Count() != 0)
            {
                TempData["error"] = "Something Went Wrong Please Try Again";
                return RedirectToAction("SignUp");
            }


            var user = new User();

            user.Name = formData.Name;
            user.Email = formData.Email;
            user.Password = formData.Password;

            db.User.Add(user);
            db.SaveChanges();
            TempData["success"] = "Successfully Created Account";

            return RedirectToAction("SignUp");
        }
    }
}
