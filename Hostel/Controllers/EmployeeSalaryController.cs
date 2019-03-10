using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class EmployeeSalaryController : Controller
    {
        // GET: EmployeeSalary
        public ActionResult Index()
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                ViewBag.Buildings = context.Buildings.ToList();
                ViewBag.TotalPay = 0;
                try
                {
                    ViewBag.TotalPay = context.EmployeeSalaries.Sum(s => s.AmmountPaid);
                }
                catch { }
                
                return View(context.EmployeeSalaries.Include("Employee").ToList());
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult SalaryCreate()
        {
            if ((string)Session["type"] == "Admin")
            {
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult SelectEmployee(string id)
        {
            if ((string)Session["type"] == "Admin")
            {

                HostelDBContext context = new HostelDBContext();
                return View(context.Employees.Include("Building").Include("Position").Where(e => e.EmployeeName == id).ToList());


            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult Searched(string id,string op="")
        {
            if ((string)Session["type"] == "Admin")
            {
                ViewBag.TotalPay = 0;
                HostelDBContext context = new HostelDBContext();
                ViewBag.Buildings = context.Buildings.ToList();
                if (op == "")
                {
                    try
                    {
                        ViewBag.TotalPay = context.EmployeeSalaries.Where(s => s.Employee.EmployeeName == id).Sum(s => s.AmmountPaid);
                    }
                    catch { }
                    return View(context.EmployeeSalaries.Include("Employee").Where(s => s.Employee.EmployeeName==id).ToList());
                }
                else
                {
                    DateTime dateto=Convert.ToDateTime(op);
                    DateTime datefrom = Convert.ToDateTime(id);
                    try
                    {
                        ViewBag.TotalPay = context.EmployeeSalaries.Where(s => s.PayDate >= datefrom && s.PayDate <= dateto).Sum(s => s.AmmountPaid);
                    }
                    catch { }
                   
                    return View(context.EmployeeSalaries.Include("Employee").Where(s => s.PayDate >= datefrom && s.PayDate<=dateto).ToList());
                }
                
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpGet]
        public ActionResult Create(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Employee emp = context.Employees.Include("Position").Include("Building").SingleOrDefault(e => e.EmployeeId == id);
                ViewBag.EmployeeId = emp.EmployeeId;
                ViewBag.EmployeeName = emp.EmployeeName;
                ViewBag.Building = emp.Building.BuildingName;
                ViewBag.Position = emp.Position.PositionName;
                ViewBag.Salary = emp.Position.Salary;
                try
                {
                    ViewBag.Paid = context.EmployeeSalaries.Where(s => s.EmployeeId == id).Sum(s => s.AmmountPaid);
                }
                catch { }
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost]
        public ActionResult Create(EmployeeSalary salary)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                salary.PayDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    context.EmployeeSalaries.Add(salary);
                    context.SaveChanges();
                    TempData["Success"] = "Successfully Salary Created";
                    return RedirectToAction("Index");
                }
                else
                {
                    Employee emp = context.Employees.Include("Position").Include("Building").SingleOrDefault(e => e.EmployeeId == salary.EmployeeId);
                    ViewBag.EmployeeId = emp.EmployeeId;
                    ViewBag.EmployeeName = emp.EmployeeName;
                    ViewBag.Building = emp.Building.BuildingName;
                    ViewBag.Position = emp.Position.PositionName;
                    ViewBag.Salary = emp.Position.Salary;
                    try
                    {
                        ViewBag.Paid = context.EmployeeSalaries.Where(s => s.EmployeeId == salary.EmployeeId).Sum(s => s.AmmountPaid);
                    }
                    catch { }
                    return View(salary);
                }
                
            }
            else
            {
                return Redirect("/User/Error");
            }
        }


    }
}