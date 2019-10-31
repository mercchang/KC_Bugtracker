using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KC_Bugtracker.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Ticket>Tickets { get; set; }
        public TicketType()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}