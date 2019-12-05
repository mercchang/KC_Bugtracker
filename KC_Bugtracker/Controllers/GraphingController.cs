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
    public class GraphingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper ticketHelper = new TicketHelper();

        public JsonResult ProduceChart1Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;

            foreach (var priority in db.TicketPriorities.ToList())
            {
                data = new MorrisBarData();
                data.label = priority.PriorityName;
                var userTickets = ticketHelper.ListMyTickets();

                data.value = userTickets.Where(t => t.TicketPriority.PriorityName == priority.PriorityName).Count();
                myData.Add(data);
            }
            return Json(myData);
        }

        public JsonResult ProduceChart2Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;
            var userTickets = ticketHelper.ListMyTickets();

            foreach (var priority in db.TicketStatuses.ToList())
            {
                data = new MorrisBarData();
                data.label = priority.StatusName;
                data.value = userTickets.Where(t => t.TicketStatus.StatusName == priority.StatusName).Count();
                myData.Add(data);
            }
            return Json(myData);
        }

        public JsonResult ProduceChart3Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;
            var userTickets = ticketHelper.ListMyTickets();

            foreach (var type in db.TicketTypes.ToList())
            {
                data = new MorrisBarData();
                data.label = type.TypeName;
                data.value = userTickets.Where(t => t.TicketType.TypeName == type.TypeName).Count();
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
