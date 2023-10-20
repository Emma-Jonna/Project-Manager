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

            var model = db.Project.Include(m => m.Material).Include(c => c.Category).Include(t => t.Type).ToList();

            foreach (var item in model)
            {
                Console.WriteLine(item.Type.Name);
            }


            return View(model);
        }

        [Authorize]
        public IActionResult UpdateProject()
        {

            var db = new project_manager_dbContext();

            var model = db.Project.ToList();

            return View(model);
        }
    }
}
