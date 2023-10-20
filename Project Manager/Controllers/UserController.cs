using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Models;
using System.Security.Claims;

namespace Project_Manager.Controllers
{
    public class UserController : Controller
    {
        public IActionResult SignIn()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var db = new project_manager_dbContext();

            var model = db.User.ToList();

            return View(model);
        }

        public IActionResult SignUp()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public async Task<IActionResult> SignOutUser()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("SignIn");
        }

        [HttpPost]
        public async Task<IActionResult> SignInUser(User formData)
        {

            if (formData.Email == null || formData.Password == null)
            {
                TempData["error"] = "Please fill in all the fields";
                return RedirectToAction("SignIn");
            }

            var db = new project_manager_dbContext();

            var user = db.User.ToList();

            var findUser = db.User.Where(u => u.Email == formData.Email && u.Password == formData.Password).ToList();

            if (findUser.Count() != 1)
            {
                TempData["error"] = "Something Went Wrong Please Try Again";
                return RedirectToAction("SignIn");
            }

            var claims = new List<Claim>
            {
                new Claim("UserId", Convert.ToString(findUser[0].Id)),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties());

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult SignUpUser(User formData)
        {

            if (formData.Email == null || formData.Password == null || formData.Name == null)
            {
                TempData["error"] = "Please fill in all the fields";
                return RedirectToAction("SignUp");
            }

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
