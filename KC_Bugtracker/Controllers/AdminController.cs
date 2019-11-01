using KC_Bugtracker.Helpers;
using KC_Bugtracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KC_Bugtracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper userRolesHelper = new UserRolesHelper();
        private ProjectsHelper projectsHelper = new ProjectsHelper();

        // GET: Admin
        public ActionResult ManageRoles()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");
            ViewBag.Role = new SelectList(db.Roles, "Name", "Name");

            var users = new List<ManageRolesViewModel>();
            foreach (var user in db.Users.ToList())
            {
                users.Add(new ManageRolesViewModel
                {
                    UserName = $"{user.LastName}, {user.FirstName}",
                    RoleName = userRolesHelper.ListUserRoles(user.Id).FirstOrDefault()
                });
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult ManageRoles(List<string> userIds, string role)
        {
            // Unenroll selected users from any roles
            foreach(var userId in userIds)
            {
                var userRole = userRolesHelper.ListUserRoles(userId).FirstOrDefault();
                if(userRole != null)
                {
                    userRolesHelper.RemoveUserFromRole(userId, userRole);
                }
            }
            // Add user back to role
            if(! string.IsNullOrEmpty(role))
            {
                foreach (var userId in userIds)
                {
                    userRolesHelper.AddUserToRole(userId, role);
                }
            }
            return RedirectToAction("ManageRoles", "Admin");
        }

        [Authorize(Roles ="Admin, ProjectManager")]
        public ActionResult ManageProjectUsers()
        {
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "Name");
            ViewBag.Developers = new MultiSelectList(userRolesHelper.UsersInRole("Developer"), "Id", "Email");
            ViewBag.Submitters = new MultiSelectList(userRolesHelper.UsersInRole("Submitter"), "Id", "Email");

            if(User.IsInRole("Admin"))
            {
                ViewBag.ProjectManagerId = new SelectList(userRolesHelper.UsersInRole("ProjectManager"), "Id", "Email");
            }

            // Create View Model for viewing users & their projects
            var myData = new List<UserProjectsListViewModel>();
            UserProjectsListViewModel userVm = null;
            foreach(var user in db.Users.ToList())
            {
                userVm = new UserProjectsListViewModel
                {
                    Name = $"{user.LastName}, {user.FirstName}",
                    ProjectNames = projectsHelper.ListUserProjects(user.Id).Select(p => p.Name).ToList()
                };

                // if no projects, add N/A
                if(userVm.ProjectNames.Count() == 0)
                    userVm.ProjectNames.Add("N/A");

                myData.Add(userVm);
            }

            return View(myData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(List<int> projects, string projectManagerId, List<string> developers, List<string> submitters)
        {
            // remove users from selected projects
            if (projects != null)
            {
                foreach (var projectId in projects)
                {
                    // remove from this project
                    foreach (var user in projectsHelper.UsersOnProject(projectId).ToList())
                    {
                        projectsHelper.RemoveUserFromProject(user.Id, projectId);
                    }

                    // add users back to projects if possible
                    if (!string.IsNullOrEmpty(projectManagerId))
                    {
                        projectsHelper.AddUserToProject(projectManagerId, projectId);
                    }
                    if (developers != null)
                    {
                        foreach (var developerId in developers)
                        {
                            projectsHelper.AddUserToProject(developerId, projectId);
                        }
                    }
                    if (submitters != null)
                    {
                        foreach (var submitterId in submitters)
                        {
                            projectsHelper.AddUserToProject(submitterId, projectId);
                        }
                    }

                }
            }

            return RedirectToAction("ManageProjectUsers");
        }


    }
}