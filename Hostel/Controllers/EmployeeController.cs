using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index(int id = 0)
        {
            if ((string)Session["type"] == "Admin")
            {

                HostelDBContext context = new HostelDBContext();
                if (id == 0)
                {
                    return View(context.Employees.Include("Building").Include("Position").ToList());
                }
                else
                {
                    return View(context.Employees.Include("Building").Include("Position").Where(e => e.BuildingId == id).ToList());
                }

            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult ManagerView()
        {
            if ((string)Session["type"] == "Manager")
            {
                int buildingid = Convert.ToInt32(Session["building"]);
                HostelDBContext context = new HostelDBContext();
                return View(context.Employees.Include("Building").Include("Position").Where(e => e.BuildingId == buildingid).ToList());
                

            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult SearchedEmployee(string id)
        {
            if ((string)Session["type"] == "Admin")
            {

                HostelDBContext context = new HostelDBContext();
                
                return View(context.Employees.Include("Building").Include("Position").Where(e => e.EmployeeName== id).ToList());
                

            }
            else
            {
                return Redirect("/User/Error");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                List<SelectListItem> BuildingList = new List<SelectListItem>();
                foreach (Building b in context.Buildings)
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = b.BuildingName;
                    li.Value = b.BuildingId.ToString();
                    BuildingList.Add(li);
                }

                List<SelectListItem> PositionList = new List<SelectListItem>();
                foreach (Position p in context.Positions)
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = p.PositionName;
                    li.Value = p.PositionId.ToString();
                    PositionList.Add(li);
                }

                ViewBag.Buildings = BuildingList;
                ViewBag.Positions = PositionList;
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                if (ModelState.IsValid)
                {
                    if (context.Users.SingleOrDefault(u => u.Username == emp.Email) != null)
                    {
                        TempData["Failed"] = "Failed to Add Employee!  Id already exists";
                        return RedirectToAction("Index/" + emp.BuildingId);
                    }
                    emp.AppointDate = DateTime.Now;
                    context.Employees.Add(emp);
                    string password = this.CreatePassword();
                    User user = new User()
                    {
                        Username =emp.Email,
                        Password = password,
                        Type = "None",
                        status = "None",
                    };
                    
                    context.Users.Add(user);
                    context.SaveChanges();
                    TempData["Success"] = "Employee Successfully Added!";
                    TempData["Info"] = "Username: " + user.Username + " ;Password: " + user.Password + " ;Access Type: " + user.Type;
                    return RedirectToAction("Index/" + emp.BuildingId);
                }
                else
                {
                    List<SelectListItem> BuildingList = new List<SelectListItem>();
                    foreach (Building b in context.Buildings)
                    {
                        SelectListItem li = new SelectListItem();
                        li.Text = b.BuildingName;
                        li.Value = b.BuildingId.ToString();
                        BuildingList.Add(li);
                    }

                    List<SelectListItem> PositionList = new List<SelectListItem>();
                    foreach (Position p in context.Positions)
                    {
                        SelectListItem li = new SelectListItem();
                        li.Text = p.PositionName;
                        li.Value = p.PositionId.ToString();
                        PositionList.Add(li);
                    }

                    ViewBag.Buildings = BuildingList;
                    ViewBag.Positions = PositionList;
                    return View(emp);
                }
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        //password generator
        public string CreatePassword()
        {
            int length = 8;
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Employee emp = context.Employees.SingleOrDefault(e => e.EmployeeId == id);
                List<SelectListItem> BuildingList = new List<SelectListItem>();
                foreach (Building b in context.Buildings)
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = b.BuildingName;
                    li.Value = b.BuildingId.ToString();
                    BuildingList.Add(li);
                }
                List<SelectListItem> PositionList = new List<SelectListItem>();
                foreach (Position p in context.Positions)
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = p.PositionName;
                    li.Value = p.PositionId.ToString();
                    PositionList.Add(li);
                }

                ViewBag.Buildings = BuildingList;
                ViewBag.Positions = PositionList;
                return View(emp);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Update(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Employee emp = context.Employees.SingleOrDefault(e => e.EmployeeId == id);
                TryUpdateModel(emp);
                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    TempData["Success"] = "Employee Info Successfully Updated!";
                    return RedirectToAction("Index/" + emp.BuildingId);
                }
                else
                {
                    List<SelectListItem> BuildingList = new List<SelectListItem>();
                    foreach (Building b in context.Buildings)
                    {
                        SelectListItem li = new SelectListItem();
                        li.Text = b.BuildingName;
                        li.Value = b.BuildingId.ToString();
                        BuildingList.Add(li);
                    }
                    List<SelectListItem> PositionList = new List<SelectListItem>();
                    foreach (Position p in context.Positions)
                    {
                        SelectListItem li = new SelectListItem();
                        li.Text = p.PositionName;
                        li.Value = p.PositionId.ToString();
                        PositionList.Add(li);
                    }

                    ViewBag.Buildings = BuildingList;
                    ViewBag.Positions = PositionList;
                    return View(emp);
                }
            }
            else
            {
                return Redirect("/User/Error");
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Employee emp = context.Employees.SingleOrDefault(e => e.EmployeeId == id);
                ViewBag.Buildings = context.Buildings.ToList();
                ViewBag.Positions = context.Positions.ToList();
                return View(emp);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Remove(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Employee emp = context.Employees.SingleOrDefault(e => e.EmployeeId == id);
                User user = context.Users.SingleOrDefault(u => u.Username == emp.Email);
                int bid = emp.BuildingId;
                context.Users.Remove(user);
                context.Employees.Remove(emp);
                context.SaveChanges();
                TempData["Success"] = "Employee Successfully Deleted!";
                return RedirectToAction("Index/" + bid);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Employee emp = context.Employees.SingleOrDefault(e => e.EmployeeId == id);
                ViewBag.Buildings = context.Buildings.ToList();
                ViewBag.Positions = context.Positions.ToList();
                return View(emp);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpGet]
        public ActionResult EmployeeProfile()
        {
            if ((string)Session["type"] == "Manager")
            {
                string email = (string)Session["username"];
                HostelDBContext context = new HostelDBContext();
                Employee emp = context.Employees.SingleOrDefault(e=>e.Email==email);
                ViewBag.Buildings = context.Buildings.ToList();
                ViewBag.Positions = context.Positions.ToList();
                return View(emp);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpGet]
        public ActionResult ManagerAdd(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Employee emp = context.Employees.SingleOrDefault(e => e.EmployeeId == id);
                User user = context.Users.SingleOrDefault(u => u.Username == emp.Email);
                user.Type = "Manager";
                user.status = "Active";
                TryUpdateModel(user);
                context.SaveChanges();
                TempData["Success"] = "Successfully Given Manager Access to "+emp.EmployeeName;
                return RedirectToAction("Index/" + emp.BuildingId);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult ManagerRemove(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Employee emp = context.Employees.SingleOrDefault(e => e.EmployeeId == id);
                User user = context.Users.SingleOrDefault(u => u.Username == emp.Email);
                user.Type = "None";
                user.status = "None";
                TryUpdateModel(user);
                context.SaveChanges();
                TempData["Success"] = "Successfully Removed Manager Access From " + emp.EmployeeName;
                return RedirectToAction("Index/" + emp.BuildingId);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }

        public ActionResult CheckAccess(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Employee emp = context.Employees.SingleOrDefault(e => e.EmployeeId == id);
                User user = context.Users.SingleOrDefault(u => u.Username == emp.Email);
                TempData["Info"] = emp.EmployeeName + " have Access: "+user.Type;
                return RedirectToAction("Index/" + emp.BuildingId);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public JsonResult Search(string term)
        {
            HostelDBContext context = new HostelDBContext();
            List<string> name = new List<string>();
            List<Employee> employeelist = context.Employees.Where(e => e.EmployeeName.Contains(term) || e.Phone.Contains(term)).ToList();
            foreach (Employee emp in employeelist)
            {
                name.Add(emp.EmployeeName);
            }

            return Json(name, JsonRequestBehavior.AllowGet);
        }


    }
}