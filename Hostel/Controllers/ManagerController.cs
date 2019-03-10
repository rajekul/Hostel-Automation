using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            if ((string)Session["type"]=="Manager") {
                int buildingid = Convert.ToInt32(Session["building"]);
                HostelDBContext context = new HostelDBContext();
                ViewBag.totalemployee = 0;
                ViewBag.totalMember = 0;
                ViewBag.seat = 0;
                ViewBag.TotalBalance = 0;
                try
                {
                    ViewBag.totalemployee = context.Employees.Where(e => e.BuildingId == buildingid).Count();
                    int totalMember = context.Members.Where(m => m.Room.BuildingId == buildingid).Count();
                    ViewBag.totalMember = totalMember;
                    ViewBag.seat = context.Rooms.Where(r => r.BuildingId == buildingid).Sum(r => r.TotalSeat) - totalMember;
                    ViewBag.TotalBalance = context.MembersPayment.Where(p => p.BuildingId == buildingid).Sum(m => m.Debit - m.Credit);
                }
                catch { }
                List<DataPoint> YearlyIncome = new List<DataPoint>{
                    new DataPoint(2800000, "2013" ),
                    new DataPoint(3500000, "2014"),
                    new DataPoint(3800000,"2015" ),
                    new DataPoint(3600000, "2016"),
                    new DataPoint(5000000, "2017"),
                };
                List<DataPoint> YearlyCost = new List<DataPoint>{
                    new DataPoint(2000000, "2013" ),
                    new DataPoint(2500000, "2014"),
                    new DataPoint(3000000,"2015" ),
                    new DataPoint(3000000, "2016"),
                    new DataPoint(3000000, "2017"),
                };

                ViewBag.YearlyIncome = JsonConvert.SerializeObject(YearlyIncome);
                ViewBag.YearlyCost = JsonConvert.SerializeObject(YearlyCost);
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
    }
}