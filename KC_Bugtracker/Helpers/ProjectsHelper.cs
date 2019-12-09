﻿using KC_Bugtracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace KC_Bugtracker.Helpers
{
    public class ProjectsHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public List<string> ListUsersOnProjectInRole(int projectId, string roleName)
        {
            var userIdList = new List<string>();

            foreach(var user in UsersOnProject(projectId))
            {
                if(roleHelper.IsUserInRole(user.Id, roleName))
                    userIdList.Add(user.Id);
            }

            return userIdList;
        }

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return (flag);
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var projects = user.Projects.ToList();
            return (projects);
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId,projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);

                if (!roleHelper.IsDemoUser(userId))
                    db.SaveChanges();
            }
        }

        public void AddPMToProject(string userId, int projectId)
        {
            Project proj = db.Projects.Find(projectId);
            
            if (db.Users.Any(u => u.Id == userId))
            {
                proj.ProjectManagerId = userId;

                if (!roleHelper.IsDemoUser(userId))
                    db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if(IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);

                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified;

                if (!roleHelper.IsDemoUser(userId))
                    db.SaveChanges();
            }
        }

        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }

        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }


    }

}
