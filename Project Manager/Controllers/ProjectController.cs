using Microsoft.AspNetCore.Mvc;

namespace Project_Manager.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateProject()
        {
            return View();
        }

        public IActionResult UpdateProject()
        {
            return View();
        }
    }
}
