using Microsoft.AspNetCore.Mvc;

namespace Project_Manager.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
    }
}
