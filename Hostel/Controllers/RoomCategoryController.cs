using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class RoomCategoryController : Controller
    {
        // GET: RoomCategory
        public ActionResult Index()
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                return View(context.RoomCategories.ToList());
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
                List<SelectListItem> TypeList = new List<SelectListItem>();
                SelectListItem li;
                li= new SelectListItem();
                li.Text = "AC";
                li.Value = "AC";
                TypeList.Add(li);
                li = new SelectListItem();
                li.Text = "Non AC";
                li.Value = "Non AC";
                TypeList.Add(li);

                ViewBag.Types = TypeList;
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost]
        public ActionResult Create(RoomCategory category)
        {
            if ((string)Session["type"] == "Admin")
            {
                if (ModelState.IsValid)
                {
                    HostelDBContext context = new HostelDBContext();
                    if (context.RoomCategories.SingleOrDefault(r => r.CategoryName == category.CategoryName && r.RoomType==category.RoomType) != null)
                    {
                        TempData["Failed"] = "Failed to Add Room Category! Room Category." + category.CategoryName + " already exists";
                        return RedirectToAction("Index"); ;
                    }
                    context.RoomCategories.Add(category);
                    context.SaveChanges();
                    TempData["Success"] = "Category Successfully Created!";
                    return RedirectToAction("Index");
                }
                else
                {
                    List<SelectListItem> TypeList = new List<SelectListItem>();
                    SelectListItem li;
                    li = new SelectListItem();
                    li.Text = "AC";
                    li.Value = "AC";
                    TypeList.Add(li);
                    li = new SelectListItem();
                    li.Text = "Non AC";
                    li.Value = "Non AC";
                    TypeList.Add(li);

                    ViewBag.Types = TypeList;
                    return View(category);
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
                RoomCategory category = context.RoomCategories.SingleOrDefault(c => c.CategoryId == id);
                List<SelectListItem> TypeList = new List<SelectListItem>();
                SelectListItem li;
                li = new SelectListItem();
                li.Text = "AC";
                li.Value = "AC";
                TypeList.Add(li);
                li = new SelectListItem();
                li.Text = "Non AC";
                li.Value = "Non AC";
                TypeList.Add(li);

                ViewBag.Types = TypeList;
                return View(category);
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
                RoomCategory category = context.RoomCategories.SingleOrDefault(c => c.CategoryId == id);
                TryUpdateModel(category);
                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    TempData["Success"] = "Category Successfully Updated!";
                    return RedirectToAction("Index");
                }
                else
                {
                    List<SelectListItem> TypeList = new List<SelectListItem>();
                    SelectListItem li;
                    li = new SelectListItem();
                    li.Text = "AC";
                    li.Value = "AC";
                    TypeList.Add(li);
                    li = new SelectListItem();
                    li.Text = "Non AC";
                    li.Value = "Non AC";
                    TypeList.Add(li);

                    ViewBag.Types = TypeList;
                    return View(category);
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
                RoomCategory category = context.RoomCategories.SingleOrDefault(c => c.CategoryId == id);
                return View(category);
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
                RoomCategory category = context.RoomCategories.SingleOrDefault(c => c.CategoryId == id);
                context.RoomCategories.Remove(category);
                context.SaveChanges();
                TempData["Success"] = "Category Successfully Deleted!";
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
                RoomCategory category = context.RoomCategories.SingleOrDefault(c=>c.CategoryId == id);
                ViewBag.Rooms = context.Rooms.Where(r => r.CategoryId == id).Count();
                ViewBag.member = context.Members.Where(m => m.Room.CategoryId == id).Count();
                return View(category);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
    }
}