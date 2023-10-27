﻿using Microsoft.AspNetCore.Authorization;
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

            foreach (var item in formData.Material)
            {
                Console.WriteLine(item.Name);
            }

            var db = new project_manager_dbContext();

            var userId = Convert.ToInt32(User.FindFirst("UserId").Value);

            if (formData.Name == null || formData.CategoryId == 0 || formData.TypeId == 0 || formData.StatusId == 0 || formData.Description == null || formData.Material.Count() < 1)
            {
                TempData["error"] = "Something Went Wrong Please Try Again";

                return RedirectToAction("CreateProject");
            }

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
                Console.WriteLine(item.Name);
                Console.WriteLine(item.Amount);

                var name = item.Name;
                var amount = item.Amount;
                var acquired = false;
                var projectId = db.Project.Max(p => p.Id);

                if (item.Acquired == true)
                {
                    Console.WriteLine("false");

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
        public IActionResult UpdateProjectInfo(Project formData)
        {
            var materials = formData.Material.ToList();

            if (User.FindFirst("UserId") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (formData.Id == 0)
            {
                TempData["error"] = "Something Went Wrong Please Try Again1";

                return RedirectToAction("Index", "Home");
            }

            var projectId = formData.Id;
            var userId = Convert.ToInt32(User.FindFirst("UserId").Value);
            var db = new project_manager_dbContext();

            var project = db.Project.First(p => p.Id == projectId);

            if (project.Id == 0)
            {
                TempData["error"] = "Something Went Wrong Please Try Again1";

                return RedirectToAction("Index", "Home");
            }

            if (formData.Name == null || formData.CategoryId == 0 || formData.TypeId == 0 || formData.StatusId == 0 || formData.Description == null)
            {
                TempData["error"] = "Something Went Wrong Please Try Again1";

                return RedirectToAction("index", new { id = projectId });
            }
            else if (formData.EndDate.HasValue && formData.StartDate == null)
            {
                TempData["error"] = "Something Went Wrong Please Try Again2";

                return RedirectToAction("Index", new { id = projectId });
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

            var projectMaterials = db.Material.Where(p => p.ProjectId == project.Id).ToList();

            foreach (var item in formData.Material)
            {
                foreach (var itemÏnDatabase in projectMaterials)
                {
                    if (itemÏnDatabase.Id == item.Id)
                    {
                        var material = db.Material.FirstOrDefault(m => m.Id == itemÏnDatabase.Id);

                        if (material != null)
                        {
                            material.Name = item.Name;
                            material.Amount = item.Amount;

                            if (item.Acquired == true)
                            {
                                material.Acquired = true;
                            }
                            else if (item.Acquired != true)
                            {
                                material.Acquired = false;
                            }

                            db.Material.Update(material);
                            db.SaveChanges();
                        }

                    }
                    else if (itemÏnDatabase.Id != item.Id)
                    {
                        var material = db.Material.FirstOrDefault(m => m.Id == itemÏnDatabase.Id);

                        if (material != null)
                        {
                            /*db.Material.Remove(material);
                            db.SaveChanges();*/

                        }
                        else if (material == null)
                        {
                            var newMaterial = new Material()
                            {
                                Name = item.Name,
                                Amount = item.Amount,
                                Acquired = item.Acquired,
                                ProjectId = project.Id,
                            };

                            Console.WriteLine(newMaterial.ToString());

                            /*db.Material.Add(newMaterial);
                            db.SaveChanges();*/
                        }
                    }
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
    }
}
