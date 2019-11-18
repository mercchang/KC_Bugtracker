using KC_Bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KC_Bugtracker.Controllers
{
    public class GraphingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public JsonResult ProduceChart1Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;
            foreach(var priority in db.TicketPriorities.ToList())
            {
                data = new MorrisBarData();
                data.label = priority.PriorityName;
                data.value = db.Tickets.Where(t => t.TicketPriority.PriorityName == priority.PriorityName).Count();
                myData.Add(data);
            }
            return Json(myData);
        }

        public JsonResult ProduceChart2Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;
            foreach (var priority in db.TicketStatuses.ToList())
            {
                data = new MorrisBarData();
                data.label = priority.StatusName;
                data.value = db.Tickets.Where(t => t.TicketStatus.StatusName == priority.StatusName).Count();
                myData.Add(data);
            }
            return Json(myData);
        }

        // GET: Graphing
        public ActionResult Index()
        {
            return View();
        }
    }
}
