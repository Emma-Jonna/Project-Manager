using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Models;

namespace Project_Manager.Controllers
{
    public class ProjectController : Controller
    {
        [Authorize]
        public IActionResult Index(int id)
        {
            var db = new project_manager_dbContext();

            //var project = db.Project.Find(id);
            //var project = db.Project.include.Where(p => p.Id == id).First();
            var project = db.Project.Include(m => m.Material).Include(c => c.Category).Include(t => t.Type).Include(s => s.Status).Where(p => p.Id == id).First();

            //Console.WriteLine(project.CategoryId);

            return View(project);
        }

        [Authorize]
        public IActionResult CreateProject()
        {
            var db = new project_manager_dbContext();

            var typeModel = db.Type.ToList();
            var categoryModel = db.Category.ToList();

            //var model = db.Category.Include.Include(c => c.Category).Include(t => t.Type).ToList();
            //var model = db.Project.Include(c => c.Category).Include(t => t.Type).ToList();

            //var model = new List<KeyValuePair<string, List<KeyValuePair<int, string>>>>();
            //var model = new List<List<KeyValuePair<int, string>>>();

            var model = new CreateProject()
            {
                Category = categoryModel,
                Type = typeModel,
            };

            return View(model);
        }

        [Authorize]
        public IActionResult UpdateProject()
        {

            var db = new project_manager_dbContext();

            var model = db.Project.ToList();

            return View(model);
        }

        public IActionResult CategoryPartial()
        {
            var db = new project_manager_dbContext();

            var categories = db.Category.ToList();

            return PartialView("_Categories", categories);
        }
    }
}
