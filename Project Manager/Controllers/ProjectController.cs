using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Models;

namespace Project_Manager.Controllers
{
    public class ProjectController : Controller
    {
        //Create Project
        //TODO if status ready all materials aquired
        //TODO if status completed all materials aquierd
        //TODO error and if things does not exist
        //TODO show success

        //Edit Project
        //TODO if status ready all materials aquired
        //TODO if status completed all materials aquierd

        [Authorize]
        public IActionResult Index(int id)
        {
            var db = new project_manager_dbContext();

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

        [HttpPost]
        [Authorize]
        public IActionResult CreateNewProject(Project formData)
        {
            if (User.FindFirst("UserId") == null)
            {
                return RedirectToAction("Index", "Home");
            }

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

            db.Project.Add(newProject);
            db.SaveChanges();

            TempData["success"] = "Successfully created project";

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateProject(int? projectId)
        {
            if (projectId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var id = projectId.Value;

            var db = new project_manager_dbContext();

            var typeModel = db.Type.ToList();
            var categoryModel = db.Category.ToList();
            var statusModel = db.Status.ToList();
            var project = db.Project.First(p => p.Id == id);

            var model = new EditProject()
            {
                Category = categoryModel,
                Type = typeModel,
                Status = statusModel,
                Project = project
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateProjectInfo(Project formData)
        {

            if (formData.Id == 0)
            {
                TempData["error"] = "Something Went Wrong Please Try Again1";

                return RedirectToAction("Index", "Project");
            }

            var projectId = formData.Id;
            var userId = Convert.ToInt32(User.FindFirst("UserId").Value);
            var db = new project_manager_dbContext();

            var project = db.Project.First(p => p.Id == projectId);

            if (project.Id == 0)
            {
                TempData["error"] = "Something Went Wrong Please Try Again1";

                return RedirectToAction("Index", "Project");
            }

            if (formData.Name == null || formData.CategoryId == 0 || formData.TypeId == 0 || formData.StatusId == 0 || formData.Description == null)
            {
                TempData["error"] = "Something Went Wrong Please Try Again1";

                return RedirectToAction("index", "Project", new { id = projectId });
            }
            else if (formData.EndDate.HasValue && formData.StartDate == null)
            {
                TempData["error"] = "Something Went Wrong Please Try Again2";

                return RedirectToAction("Index", "Project", new { id = projectId });
            }

            project.Name = formData.Name;
            project.CategoryId = formData.CategoryId;
            project.TypeId = formData.TypeId;
            project.StatusId = formData.StatusId;
            project.Description = formData.Description;
            project.StartDate = formData.StartDate;
            project.EndDate = formData.EndDate;
            project.BeforeImage = formData.BeforeImage;
            project.AfterImage = formData.AfterImage;
            project.PatternLink = formData.PatternLink;
            project.Sketch = formData.Sketch;

            db.Project.Update(project);
            db.SaveChanges();

            return RedirectToAction("index", "Project", new { id = projectId });
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteProject(int? projectId)
        {
            if (projectId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var id = projectId.Value;

            var db = new project_manager_dbContext();

            var projectToDelete = db.Project.FirstOrDefault(p => p.Id == id);
            var materialsToDelete = db.Material.FirstOrDefault(m => m.ProjectId == id);

            if (projectToDelete == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (materialsToDelete == null)
            {
                db.Project.Remove(projectToDelete);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            db.Project.Remove(projectToDelete);
            db.Material.Remove(materialsToDelete);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
