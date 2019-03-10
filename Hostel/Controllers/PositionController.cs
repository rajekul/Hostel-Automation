using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class PositionController : Controller
    {
        // GET: Position
        public ActionResult Index()
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                return View(context.Positions.ToList());
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
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost]
        public ActionResult Create(Position position)
        {
            if ((string)Session["type"] == "Admin")
            {
                if (ModelState.IsValid)
                {
                    HostelDBContext context = new HostelDBContext();
                    context.Positions.Add(position);
                    context.SaveChanges();
                    TempData["Success"] = "Position Successfully Created!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(position);
                }
            }
            else
            {
                return Redirect("/User/Error");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Position position = context.Positions.SingleOrDefault(p => p.PositionId == id);
                return View(position);
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
                Position position = context.Positions.SingleOrDefault(p => p.PositionId == id);
                TryUpdateModel(position);
                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    TempData["Success"] = "Position Successfully Updated!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(position);
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
                Position position = context.Positions.SingleOrDefault(p => p.PositionId == id);
                return View(position);
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
                Position position = context.Positions.SingleOrDefault(p => p.PositionId == id);
                context.Positions.Remove(position);
                context.SaveChanges();
                TempData["Success"] = "Position Successfully Deleted!";
                return RedirectToAction("Index");
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
                Position position = context.Positions.SingleOrDefault(p => p.PositionId == id);
                ViewBag.Employees = context.Employees.Where(e => e.PositionId == id).Count();
                return View(position);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
    }
}