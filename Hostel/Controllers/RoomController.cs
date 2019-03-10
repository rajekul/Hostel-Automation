using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class RoomController : Controller
    {
        // GET: Room
        public ActionResult Index(int id=0)
        {
            if ((string)Session["type"] == "Admin")
            {
                
                HostelDBContext context = new HostelDBContext();
                if (id == 0)
                {
                    return View(context.Rooms.Include("Building").Include("RoomCategory").ToList());
                }
                else
                {
                    return View(context.Rooms.Include("Building").Include("RoomCategory").Where(r => r.BuildingId == id).ToList());
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
                return View(context.Rooms.Include("Building").Include("RoomCategory").Where(r => r.BuildingId == buildingid).ToList());
                

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

                List<SelectListItem> CategoryList = new List<SelectListItem>();
                foreach (RoomCategory r in context.RoomCategories)
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = r.CategoryName;
                    li.Value = r.CategoryId.ToString();
                    CategoryList.Add(li);
                }

                ViewBag.Buildings = BuildingList;
                ViewBag.Categories = CategoryList;
                return View();
             }
             else
             {
                 return Redirect("/User/Error");
             }
         }
         [HttpPost]
         public ActionResult Create(Room room)
         {
             if ((string)Session["type"] == "Admin")
             {
                 HostelDBContext context = new HostelDBContext();
                 if (ModelState.IsValid)
                 {
                    if (context.Rooms.SingleOrDefault(r=>r.BuildingId==room.BuildingId && r.RoomNo==room.RoomNo) != null)
                    {
                        TempData["Failed"] = "Failed to Add Room! Room No."+room.RoomNo+" already exists in this Building";
                        return RedirectToAction("Index/" + room.BuildingId); ;
                    }
                    context.Rooms.Add(room);
                    context.SaveChanges();
                    TempData["Success"] = "Room Successfully Created!";
                    return RedirectToAction("Index/" + room.BuildingId);
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

                    List<SelectListItem> CategoryList = new List<SelectListItem>();
                    foreach (RoomCategory r in context.RoomCategories)
                    {
                        SelectListItem li = new SelectListItem();
                        li.Text = r.CategoryName;
                        li.Value = r.CategoryId.ToString();
                        CategoryList.Add(li);
                    }

                    ViewBag.Buildings = BuildingList;
                    ViewBag.Categories = CategoryList;
                    return View(room);
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
                 Room room = context.Rooms.SingleOrDefault(r => r.RoomId == id);
                 List<SelectListItem> BuildingList = new List<SelectListItem>();
                 foreach (Building b in context.Buildings)
                 {
                     SelectListItem li = new SelectListItem();
                     li.Text = b.BuildingName;
                     li.Value = b.BuildingId.ToString();
                     BuildingList.Add(li);
                 }
                List<SelectListItem> CategoryList = new List<SelectListItem>();
                foreach (RoomCategory r in context.RoomCategories)
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = r.CategoryName;
                    li.Value = r.CategoryId.ToString();
                    CategoryList.Add(li);
                }

                ViewBag.Buildings = BuildingList;
                ViewBag.Categories = CategoryList;
                return View(room);
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
                Room room = context.Rooms.SingleOrDefault(r => r.RoomId == id);
                TryUpdateModel(room);
                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    TempData["Success"] = "Room Successfully Updated!";
                    return RedirectToAction("Index/" + room.BuildingId);
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
                    List<SelectListItem> CategoryList = new List<SelectListItem>();
                    foreach (RoomCategory r in context.RoomCategories)
                    {
                        SelectListItem li = new SelectListItem();
                        li.Text = r.CategoryName;
                        li.Value = r.CategoryId.ToString();
                        CategoryList.Add(li);
                    }

                    ViewBag.Buildings = BuildingList;
                    ViewBag.Categories = CategoryList;
                    return View(room);
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
                Room room = context.Rooms.SingleOrDefault(r => r.RoomId == id);
                ViewBag.Buildings = context.Buildings.ToList();
                ViewBag.RoomCategoriess = context.RoomCategories.ToList();
                return View(room);
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
                 Room room = context.Rooms.SingleOrDefault(r => r.RoomId == id);
                int bid = room.BuildingId;
                 context.Rooms.Remove(room);
                 context.SaveChanges();
                 TempData["Success"] = "Room Successfully Deleted!";
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
                Room room = context.Rooms.Include("RoomCategory").Include("Building").SingleOrDefault(r => r.RoomId == id);
                int member = context.Members.Where(m => m.RoomId == id).Count();
                ViewBag.member = member;
                ViewBag.availSeat = room.TotalSeat - member;
                return View(room);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult ManagerDetails(int id)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Room room = context.Rooms.Include("RoomCategory").Include("Building").SingleOrDefault(r => r.RoomId == id);
                int member = context.Members.Where(m => m.RoomId == id).Count();
                ViewBag.member = member;
                ViewBag.availSeat = room.TotalSeat - member;
                return View(room);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public string RoomInfo(int id)
        {
            HostelDBContext context = new HostelDBContext();
            Room room = context.Rooms.Include("RoomCategory").SingleOrDefault(r => r.RoomId == id);
            string roomInfo = "<strong> Selected Room Information  <br/><br/> Room NO: </strong>" + room.RoomNo+ " <br/><strong>Room Category: </strong>" + room.RoomCategory.CategoryName+ " <br/><strong> Room Type: </strong>" + room.RoomCategory.RoomType+ " <br/><strong> Rent(Per Seat): </strong>" + room.RoomCategory.Rent/room.TotalSeat+ "<br/><strong> Facilities: </strong>" + room.RoomCategory.Facility;

            return roomInfo;
        }
    }
}