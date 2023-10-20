using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Models;
using System.Diagnostics;

namespace Project_Manager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            var db = new project_manager_dbContext();

            var model = db.Category.ToList();

            var uderId = Convert.ToInt32(User.FindFirst("UserId").Value);
            //var userName = User.Identity.Name;
            //var userEmail = User.Identity.Email;

            Console.WriteLine(TempData["userId"]);

            var userData = Convert.ToInt32(TempData["userId"]);

            var projects = db.Project.Include(u => u.User).Where(project => project.UserId == uderId).ToList();
            //var projects = db.Project.Include(u => u.User).Where(project => project.UserId == userData).ToList();
            //var projects = db.Project.Include(m => m.Material).Where(project => project.UserId == 1).ToList();
            //var projects = db.Project.ToList();

            return View(projects);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}