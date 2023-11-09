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
        public IActionResult CreateNewProject(Project formData)
        {
            if (User.FindFirst("UserId") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var db = new project_manager_dbContext();

            var userId = Convert.ToInt32(User.FindFirst("UserId").Value);

            if (formData.Name == null || formData.CategoryId == 0 || formData.TypeId == 0 || formData.StatusId == 0 || formData.Description == null || formData.Material.Count() < 1)
            {
                TempData["error"] = "Something Went Wrong Please Try Again";

                return RedirectToAction("CreateProject");
            }

            /*if (!string.IsNullOrEmpty(formData.BeforeImage))
            {
                //var dataStream = new MemoryStream();
                //dataStream.CopyTo(formData.BeforeImage);
                Console.WriteLine();

                using (MemoryStream mStream = new())
                {
                    //formData.BeforeImage.CopyTo(mStream);
                    //userIdentity.Pfp = mStream.ToArray();
                }
            }*/

            var newProject = new Project()
            {
                Name = formData.Name,
                CategoryId = formData.CategoryId,
                TypeId = formData.TypeId,
                StatusId = formData.StatusId,
                Description = formData.Description,
                UserId = userId,
                StartDate = formData.StartDate,
                EndDate = formData.EndDate,
                BeforeImage = formData.BeforeImage,
                AfterImage = formData.AfterImage,
                PatternLink = formData.PatternLink,
                Sketch = formData.Sketch,
            };

            db.Project.Add(newProject);
            db.SaveChanges();

            foreach (var item in formData.Material)
            {
                var name = item.Name;
                var amount = item.Amount;
                var acquired = false;
                var projectId = db.Project.Max(p => p.Id);

                if (item.Acquired == true)
                {
                    acquired = true;
                }

                var newMaterial = new Material()
                {
                    Name = name,
                    Amount = amount,
                    Acquired = acquired,
                    ProjectId = projectId,
                };

                db.Material.Add(newMaterial);
                db.SaveChanges();
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
        public IActionResult UpdateProjectInfo(EditProject formData)
        {
            var materials = formData.Material.ToList();

            //Console.WriteLine(formData.PatternFile.FileName);

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
            else if (formData.Project.Name == null || formData.Project.CategoryId == 0 || formData.Project.TypeId == 0 || formData.Project.StatusId == 0 || formData.Project.Description == null)
            {
                TempData["error"] = "All input fields needs too be filled in";

                return RedirectToAction("index", new { id = projectId });
            }
            else if (formData.Project.EndDate.HasValue && formData.Project.StartDate == null)
            {
                TempData["error"] = "If you add an end date you need to add a start date";

                return RedirectToAction("Index", new { id = projectId });
            }
            else if (formData.PatternFile != null)
            {
                if (!formData.PatternFile.FileName.Contains(".pdf"))
                {
                    TempData["error"] = "Wrong file type";

                    return RedirectToAction("Index", new { id = projectId });
                }

                project.PatternLink = CreateFilePath(formData.PatternFile, formData.Project.Id, "PatternFile");
                db.Project.Update(project);
                db.SaveChanges();
            }
            else if (formData.SketchImageFile != null)
            {
                if (!acceptedImageFiles.IsMatch(formData.SketchImageFile.FileName))
                {
                    TempData["error"] = "Wrong file type";

                    return RedirectToAction("Index", new { id = projectId });
                }

                project.Sketch = CreateFilePath(formData.SketchImageFile, formData.Project.Id, "SketchImageFile");
                db.Project.Update(project);
                db.SaveChanges();
            }
            else if (formData.BeforeImageFile != null)
            {
                if (!acceptedImageFiles.IsMatch(formData.BeforeImageFile.FileName))
                {
                    TempData["error"] = "Wrong file type";

                    return RedirectToAction("Index", new { id = projectId });
                }

                project.BeforeImage = CreateFilePath(formData.BeforeImageFile, formData.Project.Id, "BeforeImageFile");
                db.Project.Update(project);
                db.SaveChanges();

                /*var sketchImageFile = Path.Combine(AppHelper.GetImageFolder(), $"{formData.Project.Id}_SketchImageFile{Path.GetExtension(formData.SketchImageFile.FileName)}");

                using (var stream = System.IO.File.Create(sketchImageFile))
                {
                    formData.SketchImageFile.CopyToAsync(stream);
                }*/

                //project.BeforeImage = formData.BeforeImageFile.FileName;

                /*if (!string.IsNullOrEmpty(formData.BeforeImageFile.FileName))
                {
                    //var dataStream = new MemoryStream();
                    //dataStream.CopyTo(formData.BeforeImage);
                    //var imageUrl = formData.BeforeImageFile.FileName;
                    //var image = Request.Form.Files["BeforeImage"];

                    //using MemoryStream ms = new MemoryStream();
                    //using MemoryStream ms = new MemoryStream();
                    //imageUrl.CopyTo(ms);
                    //image.CopyTo(ms);

                    //using var dataStream = new MemoryStream();
                    //imageUrl.CopyTo(dataStream);

                    //var extension = Path.GetExtension(imageUrl);

                    *//*using (MemoryStream mStream = new())
                    {
                        formData.BeforeImage.CopyTo(mStream);
                        userIdentity.Pfp = mStream.ToArray();
                    }*//*                    
                    
                }*/

                /*if (HttpContext.Request.Form.Files.Count > 0)// check if the user uploaded something
                {
                    ImageFile = HttpContext.Request.Form.Files.GetFile("file_Main");
                    if (ImageFile != null)
                    {
                        var extension = Path.GetExtension(ImageFile.FileName);//get file name
                        if (ImageExtensions.Contains(extension.ToUpperInvariant()))
                        {
                            using var dataStream = new MemoryStream();
                            await ImageFile.CopyToAsync(dataStream);
                            byte[] imageBytes = dataStream.ToArray(); // you can save this to your byte array variable and remove the 2 lines below
                            string base64String = Convert.ToBase64String(imageBytes);
                            User.UserPicture = base64String; // to save the image as base64String 
                        }
                        else
                        {
                            Msg = "image format must be in JPG, BMP or PNG"; //Custom error message
                            return Page();
                        }
                    }
                }*//*
            }*/
            }
            else if (formData.AfterImageFile != null)
            {
                /*                if (formData.BeforeImageFile == null)
                                {
                                    TempData["error"] = "If you upload a after image you need to upload an before image";
                                    return RedirectToAction("Index", new { id = projectId });
                                }*/

                if (!acceptedImageFiles.IsMatch(formData.AfterImageFile.FileName))
                {
                    TempData["error"] = "Wrong file type";
                    return RedirectToAction("Index", new { id = projectId });
                }

                project.AfterImage = CreateFilePath(formData.AfterImageFile, formData.Project.Id, "AfterImageFile");
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
                    newMaterial.ProjectId = project.Id;


                    db.Material.Add(newMaterial);
                    db.SaveChanges();
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

        public string CreateFilePath(IFormFile file, int projectId, string fileName)
        {
            var imageFile = Path.Combine(AppHelper.GetImageFolder(), $"{projectId}_{fileName}{Path.GetExtension(file.FileName)}");

            using (var stream = System.IO.File.Create(imageFile))
            {
                file.CopyToAsync(stream);
            }

            return imageFile;
        }
    }
}
