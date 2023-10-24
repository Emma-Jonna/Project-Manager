using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Models;

namespace Project_Manager.Controllers
{
    public class ProjectController : Controller
    {
        //Create Project
        //TODO if status is 
        //TODO

        //Edit Project
        //TODO

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
        public IActionResult UpdateProjectInfo(EditProject formData)
        {
            var db = new project_manager_dbContext();

            var projectId = formData.Project.Id;
            var projectName = formData.Project.Name;
            var projectCategory = formData.Project.CategoryId;
            var projectType = formData.Project.TypeId;
            var projectStatus = formData.Project.StatusId;
            var projectDescription = formData.Project.Description;
            var projectStartDate = formData.Project.StartDate;
            var projectEndDate = formData.Project.EndDate;
            var projectBeforeImage = formData.Project.BeforeImage;
            var projectAfterImage = formData?.Project.AfterImage;
            var projectPatternLink = formData?.Project.PatternLink;
            var projectSketch = formData?.Project.Sketch;

            return RedirectToAction("Index", "Project", new { id = projectId });
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

            var projectToDelete = db.Project.First(p => p.Id == id);
            var materialsToDelete = db.Material.First(m => m.ProjectId == id);

            if (projectToDelete == null || materialsToDelete == null)
            {
                return RedirectToAction("Index", "Home");
            }

            db.Material.Remove(materialsToDelete);
            db.SaveChanges();
            db.Project.Remove(projectToDelete);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
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
    }
}
