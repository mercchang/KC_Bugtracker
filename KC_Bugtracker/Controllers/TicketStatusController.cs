﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KC_Bugtracker.Models;
using KC_Bugtracker.Helpers;
using Microsoft.AspNet.Identity;

namespace KC_Bugtracker.Controllers
{
    public class TicketStatusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();

        // GET: TicketStatus
        public ActionResult Index()
        {
            return View(db.TicketStatuses.ToList());
        }

        // GET: TicketStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketStatus ticketStatus = db.TicketStatuses.Find(id);
            if (ticketStatus == null)
            {
                return HttpNotFound();
            }
            return View(ticketStatus);
        }

        // GET: TicketStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TicketStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StatusName,Description")] TicketStatus ticketStatus)
        {
            if (ModelState.IsValid)
            {
                db.TicketStatuses.Add(ticketStatus);

                var userr = User.Identity.GetUserId();
                if (!rolesHelper.IsDemoUser(userr))
                    db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticketStatus);
        }

        // GET: TicketStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketStatus ticketStatus = db.TicketStatuses.Find(id);
            if (ticketStatus == null)
            {
                return HttpNotFound();
            }
            return View(ticketStatus);
        }

        // POST: TicketStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StatusName,Description")] TicketStatus ticketStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketStatus).State = EntityState.Modified;

                var userr = User.Identity.GetUserId();
                if (!rolesHelper.IsDemoUser(userr))
                    db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticketStatus);
        }

        // GET: TicketStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketStatus ticketStatus = db.TicketStatuses.Find(id);
            if (ticketStatus == null)
            {
                return HttpNotFound();
            }
            return View(ticketStatus);
        }

        // POST: TicketStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketStatus ticketStatus = db.TicketStatuses.Find(id);
            db.TicketStatuses.Remove(ticketStatus);

            var userr = User.Identity.GetUserId();
            if (!rolesHelper.IsDemoUser(userr))
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
