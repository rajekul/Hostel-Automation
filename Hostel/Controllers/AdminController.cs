using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                try
                {
                    ViewBag.totalemployee = context.Employees.Count();
                    ViewBag.totalMember = context.Members.Count();
                    ViewBag.categoryId = 0;
                    ViewBag.category = "None";
                    ViewBag.member = 0;
                    ViewBag.balance = 0;
                    int x = 0;
                    int y = 0;
                    float salarycost = 0;
                    float billingcost = 0;
                    float purchasecost = 0;

                    RoomCategory category = null;
                    foreach (RoomCategory c in context.RoomCategories)
                    {
                        HostelDBContext context1 = new HostelDBContext();
                        y = context1.Members.Where(m => m.Room.CategoryId == c.CategoryId).Count();
                        if (y > x)
                        {
                            x = y;
                            category = c;
                        }
                    }
                    salarycost = context.EmployeeSalaries.Sum(s => s.AmmountPaid);
                    billingcost = context.Bills.Sum(b => b.Ammount);
                    purchasecost = context.Purchases.Sum(p => p.Price);
                    float totalcost = salarycost + billingcost + purchasecost;
                    ViewBag.balance = context.MembersPayment.Sum(m => m.Credit) - totalcost;
                    ViewBag.categoryId = category.CategoryId;
                    ViewBag.category = category.CategoryName + "(" + category.RoomType + ")";
                    ViewBag.member = x;
                }
                catch { }

                List<DataPoint> Income = new List<DataPoint>();
                List<DataPoint> Expense = new List<DataPoint>();
                

                foreach (Building b in context.Buildings)
                {
                    float salary = 0;
                    float billing = 0;
                    float purchase = 0;
                    float income = 0;
                    HostelDBContext context1 = new HostelDBContext();
                    try
                    {
                        salary = context1.EmployeeSalaries.Where(s => s.Employee.BuildingId == b.BuildingId).Sum(s => s.AmmountPaid);
                        billing = context1.Bills.Where(bi => bi.BuildingId == b.BuildingId).Sum(bi => bi.Ammount);
                        purchase = context1.Purchases.Where(p => p.BuildingId == b.BuildingId).Sum(p => p.Price);
                        income = context1.MembersPayment.Where(m => m.BuildingId == b.BuildingId).Sum(m => m.Credit);
                    }
                    catch { }
                    float cost = salary + billing + purchase;
                    Income.Add(new DataPoint(income, b.BuildingName));
                    Expense.Add(new DataPoint(cost, b.BuildingName));
                }
                List<DataPoint> YearlyProfit = new List<DataPoint>{
                    new DataPoint(2800000, "2013" ),
                    new DataPoint(3500000, "2014"),
                    new DataPoint(3800000,"2015" ),
                    new DataPoint(3600000, "2016"),
                    new DataPoint(5000000, "2017"),
                };

                ViewBag.YearlyProfit = JsonConvert.SerializeObject(YearlyProfit);
                ViewBag.Income = JsonConvert.SerializeObject(Income);
                ViewBag.Expense = JsonConvert.SerializeObject(Expense);
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }

        public ActionResult Report(string id="",string op="")
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                float totalincome = 0;
                float totalcost = 0;
                List<Array> arraylist = new List<Array>();
                foreach (Building b in context.Buildings)
                {
                    
                    float salary = 0;
                    float billing = 0;
                    float purchase = 0;
                    float income = 0;
                    float profit = 0;
                    HostelDBContext context1 = new HostelDBContext();
                    try
                    {
                        if (id=="" && op=="")
                        {
                            salary = context1.EmployeeSalaries.Where(s => s.Employee.BuildingId == b.BuildingId).Sum(s => s.AmmountPaid);
                            billing = context1.Bills.Where(bi => bi.BuildingId == b.BuildingId).Sum(bi => bi.Ammount);
                            purchase = context1.Purchases.Where(p => p.BuildingId == b.BuildingId).Sum(p => p.Price);
                            income = context1.MembersPayment.Where(m => m.BuildingId == b.BuildingId).Sum(m => m.Credit);
                        }
                        else
                        {
                            DateTime dateto = Convert.ToDateTime(op);
                            DateTime datefrom = Convert.ToDateTime(id);
                            salary = context1.EmployeeSalaries.Where(s => s.Employee.BuildingId == b.BuildingId && s.PayDate>=datefrom && s.PayDate<=dateto).Sum(s => s.AmmountPaid);
                            billing = context1.Bills.Where(bi => bi.BuildingId == b.BuildingId && bi.BilledDate>=datefrom && bi.BilledDate<=dateto).Sum(bi => bi.Ammount);
                            purchase = context1.Purchases.Where(p => p.BuildingId == b.BuildingId && p.PurchaseDate>=datefrom && p.PurchaseDate<=dateto).Sum(p => p.Price);
                            income = context1.MembersPayment.Where(m => m.BuildingId == b.BuildingId && m.Date>=datefrom && m.Date<=dateto).Sum(m => m.Credit);
                        }
                        
                    }
                    catch { }
                    float cost = salary + billing + purchase;
                    profit = income - cost;
                    totalcost += cost;
                    totalincome += income;
                    string[] arr = new string[] { b.BuildingName, income.ToString(), cost.ToString(),profit.ToString() };
                    arraylist.Add(arr);
                }
                ViewBag.totalcost = totalcost;
                ViewBag.totalincome = totalincome;
                ViewBag.totalprofit = totalincome-totalcost;
                ViewBag.list = arraylist;
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
    }
}