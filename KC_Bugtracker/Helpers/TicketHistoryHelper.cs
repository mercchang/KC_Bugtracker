using KC_Bugtracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KC_Bugtracker.Helpers
{
    public class TicketHistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void RecordChanges(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "TicketStatusId",
                    OldValue = oldTicket.TicketStatus.StatusName,
                    NewValue = newTicket.TicketStatus.StatusName,
                    Changed = (DateTime)newTicket.Updated,
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId()
                };
                db.TicketHistories.Add(newHistoryRecord);
            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "TicketPriorityId",
                    OldValue = oldTicket.TicketPriority.PriorityName,
                    NewValue = newTicket.TicketPriority.PriorityName,
                    Changed = (DateTime)newTicket.Updated,
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId()
                };
                db.TicketHistories.Add(newHistoryRecord);
            }
            db.SaveChanges();
        }
    }
}