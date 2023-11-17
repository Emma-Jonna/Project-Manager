using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.App;
using Project_Manager.Models;
using System.Text.RegularExpressions;

namespace Project_Manager.Controllers
{
    public class ProjectController : Controller
    {
        //Create Project
        //TODO if status ready all materials acquired
        //TODO if status completed all materials acquired
        //TODO error and if things does not exist
        //TODO show success

        //Edit Project
        //TODO if status ready all materials acquired
        //TODO if status completed all materials acquired
        //TODO error and if things does not exist
        //TODO show success

        [Authorize]
        public IActionResult Index(int id)
        {
            var db = new project_manager_dbContext();

            var project = db.Project.Include(m => m.Material).Include(c => c.Category).Include(t => t.Type).Include(s => s.Status).Where(p => p.Id == id).First();

            return View(project);
        }

        public ActionResult ConvertFileToImageSource(string fileName)
        {

            var eventBytes = System.IO.File.ReadAllBytes(fileName);

            var ext = Path.GetExtension(fileName).Substring(1).ToLower();

            switch (ext)
            {
                case "jpg":
                    {
                        return File(eventBytes, "image/jpg");
                    }
                case "jpeg":
                    {
                        return File(eventBytes, "image/jpeg");
                    }
                case "png":
                    {
                        return File(eventBytes, "image/png");
                    }
                case "pdf":
                    {
                        return File(eventBytes, "application/pdf");
                    }
            }

            return File(eventBytes, "application/octet-stream");
        }


        [Authorize]
        public IActionResult CreateProject()
        {
            var db = new project_manager_dbContext();

            var typeModel = db.Type.ToList();
            var categoryModel = db.Category.ToList();
            var statusModel = db.Status.ToList();
            var materialModel = db.Material.ToList();

            var model = new CreateProject()
            {
                Category = categoryModel,
                Type = typeModel,
                Status = statusModel,
                Material = materialModel
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewProject(CreateProject formData)
        {
            if (User.FindFirst("UserId") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var db = new project_manager_dbContext();
            var userId = Convert.ToInt32(User.FindFirst("UserId").Value);
            var acceptedImageFiles = new Regex("jpeg|jpg|png");

            if (formData.Project.Name == null || formData.Project.CategoryId == 0 || formData.Project.TypeId == 0 || formData.Project.StatusId == 0 || formData.Project.Description == null || formData.Material.Count() < 1)
            {
                TempData["error"] = "Something Went Wrong Please Try Again";
                return RedirectToAction("CreateProject");
            }
            else if (formData.Project.EndDate.HasValue && formData.Project.StartDate == null)
            {
                return RedirectToPageWithMessage("error", "If you add an end date you need to add a start date", "CreateProject", "Project");
            }

            var newProject = new Project()
            {
                Name = formData.Project.Name,
                CategoryId = formData.Project.CategoryId,
                TypeId = formData.Project.TypeId,
                StatusId = formData.Project.StatusId,
                Description = formData.Project.Description,
                UserId = userId,
                StartDate = formData.Project.StartDate,
                EndDate = formData.Project.EndDate,
            };

            db.Project.Add(newProject);
            db.SaveChanges();

            var lasInsertedId = db.Project.Max(p => p.Id);

            if (formData.BeforeImageFile != null)
            {
                if (!acceptedImageFiles.IsMatch(formData.BeforeImageFile.FileName))
                {
                    return RedirectToPageWithMessage("error", "Wrong file type", "CreateProject", "Project");
                }
                newProject.BeforeImage = await CreateFilePath(formData.BeforeImageFile, lasInsertedId, "BeforeImageFile");
            }
            if (formData.AfterImageFile != null)
            {

                if (!acceptedImageFiles.IsMatch(formData.AfterImageFile.FileName))
                {
                    return RedirectToPageWithMessage("error", "Wrong file type", "CreateProject", "Project");
                }

                newProject.AfterImage = await CreateFilePath(formData.AfterImageFile, lasInsertedId, "AfterImageFile");
            }
            if (formData.SketchImageFile != null)
            {
                if (!acceptedImageFiles.IsMatch(formData.SketchImageFile.FileName))
                {
                    return RedirectToPageWithMessage("error", "Wrong file type", "CreateProject", "Project");
                }

                newProject.Sketch = await CreateFilePath(formData.SketchImageFile, lasInsertedId, "SketchImageFile");
            }
            if (formData.PatternFile != null)
            {
                if (!formData.PatternFile.FileName.Contains(".pdf"))
                {
                    return RedirectToPageWithMessage("error", "Wrong file type", "CreateProject", "Project");
                }

                newProject.PatternLink = await CreateFilePath(formData.PatternFile, lasInsertedId, "PatternFile");
            }

            db.Project.Update(newProject);
            db.SaveChanges();

            foreach (var item in formData.Material)
            {
                AddMaterialToProject(db, item, db.Project.Max(p => p.Id));
            }

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
            var material = db.Material.Where(m => m.ProjectId == id).ToList();

            var model = new EditProject()
            {
                Category = categoryModel,
                Type = typeModel,
                Status = statusModel,
                Project = project,
                Material = material
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProjectInfo(EditProject formData)
        {
            var materials = formData.Material.ToList();

            if (User.FindFirst("UserId") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (formData.Project.Id == 0)
            {
                TempData["error"] = "Something Went Wrong Please Try Again";
                return RedirectToAction("Index", "Home");
            }

            var projectId = formData.Project.Id;
            var userId = Convert.ToInt32(User.FindFirst("UserId").Value);
            var db = new project_manager_dbContext();

            var project = db.Project.First(p => p.Id == projectId);

            var acceptedImageFiles = new Regex("jpeg|jpg|png");

            if (project.Id == 0)
            {
                TempData["error"] = "Something Went Wrong Please Try Again";
                return RedirectToAction("Index", "Home");
            }

            if (formData.Project.Name == null || formData.Project.CategoryId == 0 || formData.Project.TypeId == 0 || formData.Project.StatusId == 0 || formData.Project.Description == null)
            {
                return RedirectToProjectIdPage("error", "All input fields needs too be filled in", projectId);
            }
            if (formData.Project.EndDate.HasValue && formData.Project.StartDate == null)
            {
                return RedirectToProjectIdPage("error", "If you add an end date you need to add a start date", projectId);
            }
            if (formData.BeforeImageFile != null)
            {
                if (!acceptedImageFiles.IsMatch(formData.BeforeImageFile.FileName))
                {
                    return RedirectToProjectIdPage("error", "Wrong file type", projectId);
                }

                project.BeforeImage = await CreateFilePath(formData.BeforeImageFile, formData.Project.Id, "BeforeImageFile");
            }

            if (formData.AfterImageFile != null)
            {
                if (!acceptedImageFiles.IsMatch(formData.AfterImageFile.FileName))
                {
                    return RedirectToProjectIdPage("error", "Wrong file type", projectId);
                }

                project.AfterImage = await CreateFilePath(formData.AfterImageFile, formData.Project.Id, "AfterImageFile");
            }

            if (formData.SketchImageFile != null)
            {
                if (!acceptedImageFiles.IsMatch(formData.SketchImageFile.FileName))
                {
                    return RedirectToProjectIdPage("error", "Wrong file type", projectId);
                }

                project.Sketch = await CreateFilePath(formData.SketchImageFile, formData.Project.Id, "SketchImageFile");
            }

            if (formData.PatternFile != null)
            {
                if (!formData.PatternFile.FileName.Contains(".pdf"))
                {
                    return RedirectToProjectIdPage("error", "Wrong file type", projectId);
                }

                project.PatternLink = await CreateFilePath(formData.PatternFile, formData.Project.Id, "PatternFile");
            }

            project.Name = formData.Project.Name;
            project.CategoryId = formData.Project.CategoryId;
            project.TypeId = formData.Project.TypeId;
            project.StatusId = formData.Project.StatusId;
            project.Description = formData.Project.Description;
            project.StartDate = formData.Project.StartDate;
            project.EndDate = formData.Project.EndDate;

            db.Project.Update(project);
            db.SaveChanges();

            var formMaterialList = formData.Material.ToList();
            var formMaterialsIds = new List<int>();
            var projectMaterialIds = db.Material.Where(p => p.ProjectId == project.Id).Select(x => x.Id).ToList();

            foreach (var item in formMaterialList)
            {
                formMaterialsIds.Add(item.Id);
            }

            foreach (var item in projectMaterialIds)
            {
                if (!formMaterialsIds.Contains(item))
                {
                    var materialInDatabase = db.Material.FirstOrDefault(m => m.Id == item);

                    if (materialInDatabase != null)
                    {
                        db.Material.Remove(materialInDatabase);
                        db.SaveChanges();

                    }
                }
            }

            foreach (var item in formMaterialList)
            {
                var materialInDatabase = db.Material.FirstOrDefault(m => m.Id == item.Id);

                //Adding material
                if (materialInDatabase == null || item.Id == 0)
                {
                    AddMaterialToProject(db, item, projectId);
                } //Update existing material
                else if (materialInDatabase != null)
                {
                    materialInDatabase.Name = item.Name;
                    materialInDatabase.Amount = item.Amount;

                    if (item.Acquired == true)
                    {
                        materialInDatabase.Acquired = true;
                    }
                    else
                    {
                        materialInDatabase.Acquired = false;
                    }

                    db.Material.Update(materialInDatabase);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("index", new { id = projectId });
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
            var materialsToDelete = db.Material.Where(m => m.ProjectId == id).ToList();


            if (projectToDelete == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (materialsToDelete == null)
            {
                db.Project.Remove(projectToDelete);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            foreach (var item in materialsToDelete)
            {
                db.Material.Remove(item);
                db.SaveChanges();
            }

            db.Project.Remove(projectToDelete);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public void AddMaterialToProject(project_manager_dbContext db, Material item, int projectId)
        {
            var newMaterial = new Material();

            if (item.Acquired == true)
            {
                newMaterial.Acquired = true;
            }
            else
            {
                newMaterial.Acquired = false;
            }

            newMaterial.Name = item.Name;
            newMaterial.Amount = item.Amount;
            newMaterial.ProjectId = projectId;

            db.Material.Add(newMaterial);
            db.SaveChanges();

            return;
        }

        public async Task<string> CreateFilePath(IFormFile file, int projectId, string fileName)
        {
            var imageFile = Path.Combine(AppHelper.GetImageFolder(), $"{projectId}_{fileName}{Path.GetExtension(file.FileName)}");

            using (var stream = System.IO.File.Create(imageFile))
            {
                if (Path.GetExtension(file.FileName) == ".pdf")
                {
                    stream.Position = 0;
                }

                await file.CopyToAsync(stream);
            }

            Console.WriteLine(imageFile);

            return imageFile;
        }

        public IActionResult RedirectToProjectIdPage(string messageType, string message, int projectId)
        {
            TempData[messageType] = message;
            return RedirectToAction("index", new { id = projectId });
        }

        public IActionResult RedirectToPageWithMessage(string messageType, string message, string controller, string action)
        {
            TempData[messageType] = message;
            return RedirectToAction(action, controller);
        }

        public void CheckRequiredFields(dynamic formInformation, int projectId)
        {
            var acceptedImageFiles = new Regex("jpeg|jpg|png");


        }
    }
}
