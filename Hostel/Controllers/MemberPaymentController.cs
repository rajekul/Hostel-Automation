using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class MemberPaymentController : Controller
    {
        // GET: MemberPayment
        public ActionResult Index(int id=0)
        {
            ViewBag.TotalDebit = 0;
            ViewBag.TotalCredit = 0;
            ViewBag.TotalBalance = 0;
            HostelDBContext context = new HostelDBContext();
            if ((string)Session["type"] == "Admin")
            {
                try
                {
                    ViewBag.TotalDebit = context.MembersPayment.Where(p => p.BuildingId == id).Sum(m => m.Debit);
                    ViewBag.TotalCredit = context.MembersPayment.Where(p => p.BuildingId == id).Sum(m => m.Credit);
                    ViewBag.TotalBalance = context.MembersPayment.Where(p => p.BuildingId == id).Sum(m => m.Debit - m.Credit);
                }
                catch { }
                ViewBag.buildingid = id;
                return View(context.MembersPayment.Include("Member").Where(p=>p.BuildingId==id).ToList());
            }
            else if ((string)Session["type"] == "Manager")
            {
                id = Convert.ToInt32(Session["building"]);
                try
                {
                    ViewBag.TotalDebit = context.MembersPayment.Where(p => p.BuildingId == id).Sum(m => m.Debit);
                    ViewBag.TotalCredit = context.MembersPayment.Where(p => p.BuildingId == id).Sum(m => m.Credit);
                    ViewBag.TotalBalance = context.MembersPayment.Where(p => p.BuildingId == id).Sum(m => m.Debit - m.Credit);
                }
                catch { }
                ViewBag.buildingid = id;
                return View(context.MembersPayment.Include("Member").Where(p => p.BuildingId == id).ToList());
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult Searched(int id,string op,string oop="")
        {
            ViewBag.TotalDebit = 0;
            ViewBag.TotalCredit = 0;
            ViewBag.TotalBalance = 0;
            HostelDBContext context = new HostelDBContext();
            if ((string)Session["type"] == "Admin")
            {
               
                if (oop == "")
                {
                    try
                    {
                        ViewBag.TotalDebit = context.MembersPayment.Where(p => p.Member.MemberName == op && p.BuildingId == id).Sum(m => m.Debit);
                        ViewBag.TotalCredit = context.MembersPayment.Where(p => p.Member.MemberName == op && p.BuildingId == id).Sum(m => m.Credit);
                        ViewBag.TotalBalance = context.MembersPayment.Where(p => p.Member.MemberName == op && p.BuildingId == id).Sum(m => m.Debit - m.Credit);
                    }
                    catch { }
                    ViewBag.buildingid = id;
                    return View(context.MembersPayment.Include("Member").Where(p => p.Member.MemberName == op && p.BuildingId == id).ToList());
                }
                else
                {
                    DateTime dateto = Convert.ToDateTime(oop);
                    DateTime datefrom = Convert.ToDateTime(op);
                    try
                    {
                        ViewBag.TotalDebit = context.MembersPayment.Where(p => p.Date >= datefrom && p.Date <= dateto && p.BuildingId == id).Sum(m => m.Debit);
                        ViewBag.TotalCredit = context.MembersPayment.Where(p => p.Date >= datefrom && p.Date <= dateto && p.BuildingId == id).Sum(m => m.Credit);
                        ViewBag.TotalBalance = context.MembersPayment.Where(p => p.Date >= datefrom && p.Date <= dateto && p.BuildingId == id).Sum(m => m.Debit - m.Credit);
                    }
                    catch { }
                    ViewBag.buildingid = id;
                    return View(context.MembersPayment.Include("Member").Where(p=> p.Date>=datefrom && p.Date<=dateto && p.BuildingId == id).ToList());
                }
                
            }
            else if ((string)Session["type"] == "Manager")
            {
                id = Convert.ToInt32(Session["building"]);
                
                if (oop == "")
                {
                    try
                    {
                        ViewBag.TotalDebit = context.MembersPayment.Where(p => p.Member.MemberName == op && p.BuildingId == id).Sum(m => m.Debit);
                        ViewBag.TotalCredit = context.MembersPayment.Where(p => p.Member.MemberName == op && p.BuildingId == id).Sum(m => m.Credit);
                        ViewBag.TotalBalance = context.MembersPayment.Where(p => p.Member.MemberName == op && p.BuildingId == id).Sum(m => m.Debit - m.Credit);
                    }
                    catch { }
                    ViewBag.buildingid = id;
                    return View(context.MembersPayment.Include("Member").Where(p => p.Member.MemberName == op && p.BuildingId == id).ToList());
                }
                else
                {
                    DateTime dateto = Convert.ToDateTime(oop);
                    DateTime datefrom = Convert.ToDateTime(op);
                    try
                    {
                        ViewBag.TotalDebit = context.MembersPayment.Where(p => p.Date >= datefrom && p.Date <= dateto && p.BuildingId == id).Sum(m => m.Debit);
                        ViewBag.TotalCredit = context.MembersPayment.Where(p => p.Date >= datefrom && p.Date <= dateto && p.BuildingId == id).Sum(m => m.Credit);
                        ViewBag.TotalBalance = context.MembersPayment.Where(p => p.Date >= datefrom && p.Date <= dateto && p.BuildingId == id).Sum(m => m.Debit - m.Credit);
                    }
                    catch { }
                    ViewBag.buildingid = id;
                    return View(context.MembersPayment.Include("Member").Where(p => p.Date >= datefrom && p.Date <= dateto && p.BuildingId == id).ToList());
                }

            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult SelectMember(string id)
        {
            if ((string)Session["type"] == "Manager")
            {
                int buildingid = Convert.ToInt32(Session["building"]);
                HostelDBContext context = new HostelDBContext();
                return View(context.Members.Include("Room").Where(m => m.Room.BuildingId == buildingid && m.MemberName == id).ToList());
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpGet]
        public ActionResult CreatePayment()
        {
            if ((string)Session["type"] == "Manager")
            {
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpGet]
        public ActionResult Create(int id)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Member member = context.Members.Include("Room").SingleOrDefault(m => m.MemberId == id);
                Room room = context.Rooms.Include("Building").Include("RoomCategory").SingleOrDefault(r => r.RoomId == member.RoomId);
                ViewBag.MemberId = member.MemberId;
                ViewBag.MemberName = member.MemberName;
                ViewBag.balance = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Balance);
                ViewBag.bill = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Debit);
                ViewBag.pay = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Credit);
                ViewBag.Rent = room.RoomCategory.Rent / room.TotalSeat;
                ViewBag.Room = room.RoomNo;
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost]
        public ActionResult Create(MemberPayment payment)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                payment.BuildingId = Convert.ToInt32(Session["building"]);
                payment.Date = DateTime.Now;
                payment.Debit = 0;
                payment.Balance = context.MembersPayment.Where(p => p.MemberId == payment.MemberId).Sum(b => b.Balance)-payment.Credit;
                if (ModelState.IsValid)
                {
                    
                    context.MembersPayment.Add(payment);
                    context.SaveChanges();
                    TempData["Success"] = "Payment Successfully Created!";
                    return RedirectToAction("Index");
                }
                else
                {
                    Member member = context.Members.Include("Room").SingleOrDefault(m => m.MemberId == payment.MemberId);
                    Room room = context.Rooms.Include("Building").Include("RoomCategory").SingleOrDefault(r => r.RoomId == member.RoomId);
                    ViewBag.MemberId = member.MemberId;
                    ViewBag.MemberName = member.MemberName;
                    ViewBag.balance = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Balance);
                    ViewBag.bill = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Debit);
                    ViewBag.pay = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Credit);
                    ViewBag.Rent = room.RoomCategory.Rent / room.TotalSeat;
                    ViewBag.Room = room.RoomNo;
                    return View(payment);
                }
            }
            else
            {
                return Redirect("/User/Error");
            }
        }

    }
}