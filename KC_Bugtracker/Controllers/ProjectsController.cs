using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KC_Bugtracker.Helpers;
using KC_Bugtracker.Models;
using Microsoft.AspNet.Identity;

namespace KC_Bugtracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new Helpers.UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();

        [Authorize(Roles = "Admin, ProjectManager, DemoAdmin, DemoProjectManager")]
        public ActionResult ManageUsers(int id)
        {
            ViewBag.ProjectId = id;
            ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("ProjectManager"), "Id", "Email", projectHelper.ListUsersOnProjectInRole(id, "ProjectManager").FirstOrDefault());

            ViewBag.Developers = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "Email", projectHelper.ListUsersOnProjectInRole(id, "Developer"));

            ViewBag.Submitters = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "Email", projectHelper.ListUsersOnProjectInRole(id, "Submitter"));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult ManageUsers(int projectId, string projectManagerId, List<string> developers, List<string> submitters)
        {
            foreach(var user in projectHelper.UsersOnProject(projectId).ToList())
            {
                projectHelper.RemoveUserFromProject(user.Id, projectId);
            }

            if(!string.IsNullOrEmpty(projectManagerId))
            {
                projectHelper.AddUserToProject(projectManagerId, projectId);
            }

            if(developers != null)
            {
                foreach(var developerId in developers)
                {
                    projectHelper.AddUserToProject(developerId, projectId);
                }
            }

            if (submitters != null)
            {
                foreach (var submitterId in submitters)
                {
                    projectHelper.AddUserToProject(submitterId, projectId);
                }
            }

            return RedirectToAction("ManageUsers", new { id = projectId });
        }

        // GET: Projects
        [Authorize]
        public ActionResult Index()
        {
            // displays user's projects. Admin sees all projects
            if (!(User.IsInRole("Admin") || User.IsInRole("DemoAdmin")))
            {
                var userProjects = projectHelper.ListUserProjects(User.Identity.GetUserId());
                return View(userProjects.ToList());
            }
            else
                return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            ProjectViewModel model = new ProjectViewModel();


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            model.Project = db.Projects.Find(id);
            model.ProjectManager = db.Users.FirstOrDefault(m => m.Id == model.Project.ProjectManagerId);

            return View(model);
        }

        [Authorize(Roles = "Admin, DemoAdmin")]
        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {
            
            if (ModelState.IsValid)
            {
                project.Created = DateTime.Now;
                db.Projects.Add(project);
                var userr = User.Identity.GetUserId();
                if (!roleHelper.IsDemoUser(userr))
                    db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Created")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Updated = DateTime.Now;
                db.Entry(project).State = EntityState.Modified;
                var userr = User.Identity.GetUserId();
                if (!roleHelper.IsDemoUser(userr))
                    db.SaveChanges();
                return RedirectToAction("Index", "Projects");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            var userr = User.Identity.GetUserId();
            if (!roleHelper.IsDemoUser(userr))
                db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
