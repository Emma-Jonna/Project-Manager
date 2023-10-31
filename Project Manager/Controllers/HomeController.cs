using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Models;

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

            if (User.FindFirst("UserId").Value == null)
            {
                return RedirectToAction("SignIn", "User");
            }

            var userId = Convert.ToInt32(User.FindFirst("UserId").Value);

            var projects = db.Project.Include(u => u.User).Include(c => c.Category).Include(t => t.Type).Include(s => s.Status).Where(project => project.UserId == userId).ToList();

            return View(projects);
        }

        /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}