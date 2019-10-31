using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KC_Bugtracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string RecipientId { get; set; }

        public string NotificationBody { get; set; }
        public bool IsRead { get; set; }

        public virtual ApplicationUser Recipient { get; set; }
        public virtual ICollection<Ticket>Tickets { get; set; }

        public TicketNotification()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}