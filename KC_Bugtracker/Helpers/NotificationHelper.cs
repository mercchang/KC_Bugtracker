using KC_Bugtracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KC_Bugtracker.Helpers
{
    public class NotificationHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public void ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            var ticketAssigned = oldTicket.DeveloperId == null && newTicket.DeveloperId != null;
            var ticketUnAssigned = oldTicket.DeveloperId != null && newTicket.DeveloperId == null;
            var ticketReAssigned = oldTicket.DeveloperId != null && newTicket.DeveloperId != null && oldTicket.DeveloperId != newTicket.DeveloperId;

            if (ticketAssigned)
                AssignmentNotification(oldTicket, newTicket);
            else if (ticketUnAssigned)
                UnAssignmentNotification(oldTicket, newTicket);
            else if (ticketReAssigned)
            {
                UnAssignmentNotification(oldTicket, newTicket);
                AssignmentNotification(oldTicket, newTicket);
            }
        }

        private void AssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                IsRead = false,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = newTicket.DeveloperId,
                Created = DateTime.Now,
                NotificationBody = $"You have been assigned to <b>Ticket</b> #{newTicket.Id}, for the <b>{newTicket.Project.Name}</b>."
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        private void UnAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                IsRead = false,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = oldTicket.DeveloperId,
                Created = DateTime.Now,
                NotificationBody = $"You have been unassigned from <b>Ticket</b> #{newTicket.Id}, for the <b>{newTicket.Project.Name}</b>."
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        public static List<TicketNotification> GetUnreadNotifications()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Include("Sender").Include("Recipient").Where(t => t.RecipientId == currentUserId && !t.IsRead).ToList();
        }
    }
}