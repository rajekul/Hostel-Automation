using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class BillController : Controller
    {
        // GET: Bill
        public ActionResult Index(int id=0)
        {
            ViewBag.TotalBill = 0;
            HostelDBContext context = new HostelDBContext();
            ViewBag.Buildings = context.Buildings.ToList();
            if ((string)Session["type"] == "Admin")
            {
                
                try
                {
                    ViewBag.TotalBill = context.Bills.Where(b => b.BuildingId == id).Sum(b => b.Ammount);
                }
                catch { }
                ViewBag.BuildingId = id;
                return View(context.Bills.Include("Building").Where(b => b.BuildingId == id).ToList());
            }
            else if ((string)Session["type"] == "Manager")
            {
                id = Convert.ToInt32(Session["building"]);
                try
                {
                    ViewBag.TotalBill = context.Bills.Where(b => b.BuildingId == id).Sum(b => b.Ammount);
                }
                catch { }
                ViewBag.BuildingId = id;
                return View(context.Bills.Include("Building").Where(b => b.BuildingId == id).ToList());
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult Searched(int id, string op,string oop)
        {
            ViewBag.TotalBill = 0;
            HostelDBContext context = new HostelDBContext();
            ViewBag.Buildings = context.Buildings.ToList();
            DateTime dateto = Convert.ToDateTime(oop);
            DateTime datefrom = Convert.ToDateTime(op);
            if ((string)Session["type"] == "Admin" || (string)Session["type"] == "Manager")
            {
                if ((string)Session["type"] == "Manager")
                {
                    id = Convert.ToInt32(Session["building"]);
                }
                try
                {
                    ViewBag.TotalBill = context.Bills.Where(b=>b.BilledDate>=datefrom && b.BilledDate<=dateto && b.BuildingId==id).Sum(b => b.Ammount);
                }
                catch { }
                ViewBag.BuildingId = id;
                return View(context.Bills.Include("Building").Where(b => b.BilledDate >= datefrom && b.BilledDate <= dateto && b.BuildingId == id).ToList());

            }
            
                
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpGet]
        public ActionResult Create(int id=0)
        {
            if ((string)Session["type"] == "Admin")
            {
                ViewBag.BuildingId = id;
                return View();
            }
            else if ((string)Session["type"] == "Manager")
            {
                ViewBag.BuildingId=Convert.ToInt32(Session["building"]);
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost]
        public ActionResult Create(Bill bill)
        {
            if ((string)Session["type"] == "Admin" || (string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                bill.BilledDate = DateTime.Now;
                bill.SubmitBy = Session["type"].ToString();
                
                if (ModelState.IsValid)
                {
                    
                    context.Bills.Add(bill);
                    context.SaveChanges();
                    TempData["Success"] = "Bill Successfully Created!";
                    return RedirectToAction("Index/"+bill.BuildingId);
                }
                else
                {
                    ViewBag.BuildingId = bill.BuildingId;
                    return View(bill);
                }
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
    }
}