using KC_Bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KC_Bugtracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
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
                RecipientId = newTicket.DeveloperId,
                NotificationBody = $"You have been assigned to a Ticket: {newTicket.Id}, for project: {newTicket.Project.Name}."
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
                RecipientId = newTicket.DeveloperId,
                NotificationBody = $"You have been unassigned from a Ticket: {newTicket.Id}, for project: {newTicket.Project.Name}."
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }
    }
}