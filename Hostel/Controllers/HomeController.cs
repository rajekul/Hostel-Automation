using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            HostelDBContext context = new HostelDBContext();
            Hostel hostel = context.Hostels.SingleOrDefault(h => h.HostelId == 1);
            ViewBag.hostelname = hostel.HostelName;
            ViewBag.title = hostel.HostelTitle;
            ViewBag.phone = hostel.Phone;
            ViewBag.email = hostel.Email;
            ViewBag.city = hostel.City;
            ViewBag.area = hostel.Area;
            ViewBag.road = hostel.Road;
            ViewBag.house = hostel.House;
            return View();
        }
    }
}