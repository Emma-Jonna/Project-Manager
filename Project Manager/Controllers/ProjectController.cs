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

            return View(project);
        }

        [Authorize]
        public IActionResult CreateProject()
        {
            var db = new project_manager_dbContext();

            var typeModel = db.Type.ToList();
            var categoryModel = db.Category.ToList();
            var statusModel = db.Status.ToList();

            var model = new CreateProject()
            {
                Category = categoryModel,
                Type = typeModel,
                Status = statusModel,
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

        [HttpPost]
        public IActionResult DeleteProject(int? projectId)
        {
            if (projectId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var id = projectId.Value;

            var db = new project_manager_dbContext();

            var projectToDelete = db.Project.Find(id);
            var materialsToDelete = db.Material.Find(id);

            if (projectToDelete == null || materialsToDelete == null)
            {
                return RedirectToAction("Index", "Home");
            }

            db.Project.Remove(projectToDelete);
            db.Material.Remove(materialsToDelete);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewProject(Project formData)
        {
            var db = new project_manager_dbContext();

            var projectName = formData.Name;
            var category = formData.CategoryId;
            var type = formData.TypeId;
            var status = formData.StatusId;
            var description = formData.Description;
            var userId = Convert.ToInt32(User.FindFirst("UserId").Value);

            if (projectName == null || category == 0 || type == 0 || status == 0 || description == null)
            {
                TempData["error"] = "Something Went Wrong Please Try Again";

                return RedirectToAction("CreateProject", "Project");
            }



            var newProject = new Project()
            {
                Name = projectName,
                CategoryId = category,
                TypeId = type,
                StatusId = status,
                Description = description,
                UserId = userId,
                StartDate = null,
                EndDate = null,
                BeforeImage = "",
                AfterImage = "",
                PatternLink = "",
                Sketch = ""
            };

            Console.WriteLine(newProject);

            Console.WriteLine(formData.Name);
            Console.WriteLine(formData.CategoryId);
            Console.WriteLine(formData.TypeId);
            Console.WriteLine(formData.StatusId);

            db.Project.Add(newProject);
            db.SaveChanges();

            TempData["success"] = "Successfully created project";

            return RedirectToAction("Index", "Home");
        }
    }
}
