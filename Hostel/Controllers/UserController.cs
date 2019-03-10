using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            HostelDBContext context = new HostelDBContext();
            Hostel hostel = context.Hostels.SingleOrDefault(h => h.HostelId == 1);
            ViewBag.hostelname = hostel.HostelName;
            ViewBag.hostellogo = hostel.HostelLogo;
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection coll)
        {
            string username = coll["username"];
            string password = coll["password"];
            HostelDBContext context = new HostelDBContext();
            Hostel hostel = context.Hostels.SingleOrDefault(h => h.HostelId == 1);
            ViewBag.hostelname = hostel.HostelName;
            ViewBag.hostellogo = hostel.HostelLogo;
            if (username == "")
            {
                ViewBag.message = "<div class='alert alert-danger'><strong>Oops!!</strong> Username cannot be empty.</div>";
                ViewBag.username = username;
                return View();
            }
            else
            {
                
                User user = context.Users.SingleOrDefault(u => u.Username== username);
                if (user!=null)
                 {
                     if (user.Password == password)
                      {
                        if (user.status == "Active")
                        {
                            
                            Session["HostelTitle"] = hostel.HostelTitle;
                            Session["username"] = username;
                            Session["type"] = (string)user.Type;
                            if (user.Type == "Admin")
                            {

                                List<SelectListItem> BuildingList = new List<SelectListItem>();
                                foreach (Building b in context.Buildings)
                                {
                                    SelectListItem li = new SelectListItem();
                                    li.Text = b.BuildingName;
                                    li.Value = b.BuildingId.ToString();
                                    BuildingList.Add(li);
                                }
                                Session["BuildingList"] =BuildingList;
                                return Redirect("/Admin");
                            }
                            else if(user.Type=="Manager")
                            {
                                Employee emp=context.Employees.SingleOrDefault(e => e.Email == user.Username);
                                Session["name"] = emp.EmployeeName;
                                Session["building"] = emp.BuildingId;
                                return Redirect("/Manager");
                            }
                            else if(user.Type=="Member")
                            {
                                return Redirect("/Manager");
                            }
                            else
                            {
                                ViewBag.message = "<div class='alert alert-danger'><strong>Oops!!</strong> You don't have any access. Please Contact with the Admin</div>";
                                ViewBag.username = username;
                                return View();
                            }
                        }
                        else
                        {
                            ViewBag.message = "<div class='alert alert-danger'><strong>Oops!!</strong> You don't have any access. Please Contact with the Admin</div>";
                            ViewBag.username = username;
                            return View();
                        }
                      }
                      else
                      {
                        ViewBag.message = "<div class='alert alert-danger'><strong>Oops!!</strong> Incorrect Username or Password</div>";
                        ViewBag.username = username;
                        return View();
                    }
                
                }
                else
                {
                    ViewBag.message = "<div class='alert alert-danger'><strong>Oops!!</strong> Incorrect Username or Password</div>";
                    ViewBag.username = username;
                    return View();
                }
            }
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            if ((string)Session["type"]=="Admin" || (string)Session["type"] == "Manager" || (string)Session["type"] == "Member") {
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection coll)
        {
            if ((string)Session["type"] == "Admin" || (string)Session["type"] == "Manager" || (string)Session["type"] == "Member")
            {
                string username = (string)Session["username"];
                string oldpassword = coll["oldpassword"];
                string newpassword = coll["newpassword"];
                string confirmpassword = coll["confirmpassword"];
                HostelDBContext context = new HostelDBContext();
                User user = context.Users.SingleOrDefault(u => u.Username == username);
                if(oldpassword!="" && newpassword!="" && confirmpassword != "")
                {
                    if (user.Password == oldpassword)
                    {
                        if (newpassword == confirmpassword)
                        {
                            user.Password = newpassword;
                            TryUpdateModel(user);
                            context.SaveChanges();
                            TempData["Success"] = "Password Successfully Changed";
                            return RedirectToAction("ChangePassword");
                        }
                        else
                        {
                            ViewBag.message = "<div class='alert alert-danger'><strong>Oops!!</strong> Confirm Password Not Matched</div>";
                        }
                    }
                    else
                    {
                        ViewBag.message = "<div class='alert alert-danger'><strong>Error!!</strong> Incorrect Password</div>";
                    }
                }
                else
                {
                    ViewBag.message = "<div class='alert alert-danger'><strong>Error!!</strong> All Fields Required</div>";
                }
                ViewBag.oldpassword = coll["oldpassword"];
                ViewBag.newpassword = coll["newpassword"];
                ViewBag.confirmpassword = coll["confirmpassword"];
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}