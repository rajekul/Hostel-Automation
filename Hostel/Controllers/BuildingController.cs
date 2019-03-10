using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class BuildingController : Controller
    {
        // GET: Building
        public ActionResult Index()
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                return View(context.Buildings.ToList());
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
        public ActionResult Create(Building building)
        {
            if ((string)Session["type"] == "Admin")
            {
                if (ModelState.IsValid)
                {
                    HostelDBContext context = new HostelDBContext();
                    if (context.Buildings.SingleOrDefault(b => b.BuildingName == building.BuildingName) != null)
                    {
                        TempData["Failed"] = "Failed to Add Building! Building: " + building.BuildingName + " already exists";
                        return RedirectToAction("Index"); 
                    }
                    context.Buildings.Add(building);
                    context.SaveChanges();
                    List<SelectListItem> BuildingList = new List<SelectListItem>();
                    foreach (Building b in context.Buildings)
                    {
                        SelectListItem li = new SelectListItem();
                        li.Text = b.BuildingName;
                        li.Value = b.BuildingId.ToString();
                        BuildingList.Add(li);
                    }
                    Session["BuildingList"] = BuildingList;
                    TempData["Success"] = "Building Successfully Created!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(building);
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
                Building building = context.Buildings.SingleOrDefault(b => b.BuildingId == id);
                return View(building);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost,ActionName("Edit")]
        public ActionResult Update(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Building building = context.Buildings.SingleOrDefault(b => b.BuildingId == id);
                TryUpdateModel(building);
                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    TempData["Success"] = "Building Successfully Updated!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(building);
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
                Building building = context.Buildings.SingleOrDefault(b => b.BuildingId == id);
                return View(building);
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
                Building building = context.Buildings.SingleOrDefault(b => b.BuildingId == id);
                context.Buildings.Remove(building);
                context.SaveChanges();
                List<SelectListItem> BuildingList = new List<SelectListItem>();
                foreach (Building b in context.Buildings)
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = b.BuildingName;
                    li.Value = b.BuildingId.ToString();
                    BuildingList.Add(li);
                }
                Session["BuildingList"] = BuildingList;
                TempData["Success"] = "Building Successfully Deleted!";
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
                Building building = context.Buildings.SingleOrDefault(b => b.BuildingId == id);
                ViewBag.Rooms = context.Rooms.Where(r=>r.BuildingId==id).Count();
                ViewBag.member = context.Members.Where(m => m.Room.BuildingId == id).Count();
                return View(building);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
    }
}