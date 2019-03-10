using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult Index()
        {
            if ((string)Session["type"] == "Manager")
            {
                int buildingid = Convert.ToInt32(Session["building"]);
                HostelDBContext context = new HostelDBContext();
                return View(context.Members.Include("Room").Where(m=>m.Room.BuildingId==buildingid).ToList());
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult Searched(string id)
        {
            if ((string)Session["type"] == "Manager")
            {
                int buildingid = Convert.ToInt32(Session["building"]);
                HostelDBContext context = new HostelDBContext();
                return View(context.Members.Include("Room").Where(m => m.Room.BuildingId == buildingid && m.MemberName==id).ToList());
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult AdminView(int id=0)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                if (id == 0)
                {
                    return View(context.Members.Include("Room").ToList());
                }
                return View(context.Members.Include("Room").Where(m => m.Room.BuildingId == id).ToList());
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        public ActionResult AdminSearched(string id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                
                return View(context.Members.Include("Room").Where(m => m.MemberName == id).ToList());
            }
            else
            {
                return Redirect("/User/Error");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            if ((string)Session["type"] == "Manager")
            {
                int buildingid = Convert.ToInt32(Session["building"]);
                HostelDBContext context = new HostelDBContext();
                List<SelectListItem> RoomList = new List<SelectListItem>();
                SelectListItem l = new SelectListItem();
                l.Text = "Select";
                l.Value = "";
                RoomList.Add(l);
                foreach (Room room in context.Rooms.Where(r=>r.BuildingId==buildingid && r.Members.Count< r.TotalSeat))
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = room.RoomNo;
                    li.Value = room.RoomId.ToString();
                    RoomList.Add(li);
                }
                ViewBag.Rooms = RoomList;
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost]
        public ActionResult Create(Member member, HttpPostedFileBase file)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Room room = context.Rooms.Include("RoomCategory").SingleOrDefault(r => r.RoomId == member.RoomId);
                if (ModelState.IsValid)
                {
                    if (context.Users.SingleOrDefault(u => u.Username == member.Email) != null)
                    {
                        TempData["Failed"] = "Failed to Add Member!  Email already exists";
                        return RedirectToAction("Index");
                    }
                    string password = this.CreatePassword();
                    User user = new User()
                    {
                        Username = member.Email,
                        Password = password,
                        Type = "Member",
                        status = "Active",
                    };
                    MemberPayment payment = new MemberPayment()
                    {
                        Date = DateTime.Now,
                        Details = "Rent For Month" + DateTime.Now.ToString("MMMM"),
                        Debit = room.RoomCategory.Rent/room.TotalSeat,
                        Credit = 0,
                        Balance = room.RoomCategory.Rent / room.TotalSeat,
                        Member = member,
                        BuildingId=Convert.ToInt32(Session["building"]),
                    };
                    try
                    {

                        if (Request.Files.Count > 0)
                        {
                            var Inputfile = Request.Files[0];

                            if (Inputfile != null && Inputfile.ContentLength > 0)
                            {
                                
                                var filename = Path.GetFileName(Inputfile.FileName);
                                var path = Path.Combine(Server.MapPath("~/Uploads/Pictures"), member.Email + Path.GetExtension(Inputfile.FileName));
                                Inputfile.SaveAs(path);
                                member.Photo = member.Email + Path.GetExtension(Inputfile.FileName);
                            }

                        }

                    }
                    catch (Exception)
                    {

                        TempData["Failed"] = "Failed To Upload Logo";

                    }
                    member.JoinDate = DateTime.Now;
                    context.Members.Add(member);
                    context.MembersPayment.Add(payment);
                    context.Users.Add(user);
                    context.SaveChanges();
                    TempData["Success"] = "Member Successfully Added!";
                    TempData["Info"] = "Username: " + user.Username + " ;Password: " + user.Password ;
                    return RedirectToAction("Index");
                }
                else
                {
                    int buildingid = Convert.ToInt32(Session["building"]);
                    List<SelectListItem> RoomList = new List<SelectListItem>();
                    SelectListItem l = new SelectListItem();
                    l.Text = "Select";
                    l.Value = "";
                    RoomList.Add(l);
                    foreach (Room rooms in context.Rooms.Where(r => r.BuildingId == buildingid && r.Members.Count < r.TotalSeat))
                    {
                        SelectListItem li = new SelectListItem();
                        li.Text = rooms.RoomNo;
                        li.Value = rooms.RoomId.ToString();
                        RoomList.Add(li);
                    }
                    ViewBag.Rooms = RoomList;
                    return View();
                }
            }
            else
            {
                return Redirect("/User/Error");
            }
        }

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
            if ((string)Session["type"] == "Manager")
            {
                
                HostelDBContext context = new HostelDBContext();
                Member member = context.Members.SingleOrDefault(m=> m.MemberId==id);
                return View(member);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Update(int id, HttpPostedFileBase file)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Member member = context.Members.SingleOrDefault(m => m.MemberId == id);
                try
                {

                    if (Request.Files.Count > 0)
                    {
                        var Inputfile = Request.Files[0];

                        if (Inputfile != null && Inputfile.ContentLength > 0)
                        {
                            if (System.IO.File.Exists("~/Uploads/Pictures" + member.Photo))
                            {
                                System.IO.File.Delete("~/Uploads/Pictures" + member.Photo);
                            }
                            var filename = Path.GetFileName(Inputfile.FileName);
                            var path = Path.Combine(Server.MapPath("~/Uploads/Pictures"), member.Email + Path.GetExtension(Inputfile.FileName));
                            Inputfile.SaveAs(path);
                            member.Photo = member.Email + Path.GetExtension(Inputfile.FileName);
                        }

                    }

                }
                catch (Exception)
                {

                    TempData["Failed"] = "Failed To Upload Logo";

                }
                TryUpdateModel(member);
                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    TempData["Success"] = "Member Successfully Updated!";
                    return RedirectToAction("Index");
                }
                else
                {
                   
                    return View(member);
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
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Member member = context.Members.SingleOrDefault(m => m.MemberId == id);
                ViewBag.balance = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Debit-b.Credit);
                ViewBag.Rooms = context.Rooms.ToList();
                return View(member);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Remove(int id)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Member member = context.Members.SingleOrDefault(m => m.MemberId == id);
                User user = context.Users.SingleOrDefault(u => u.Username == member.Email);
                context.Users.Remove(user);
                context.Members.Remove(member);
                context.SaveChanges();
                TempData["Success"] = "Member Successfully Deleted!";
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
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Member member = context.Members.Include("Room").SingleOrDefault(m => m.MemberId == id);
                Room room = context.Rooms.Include("Building").Include("RoomCategory").SingleOrDefault(r => r.RoomId == member.RoomId);
                ViewBag.balance = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Balance);
                ViewBag.bill = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Debit);
                ViewBag.pay = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Credit);
                ViewBag.Building = room.Building.BuildingName;
                ViewBag.Rent = room.RoomCategory.Rent / room.TotalSeat;
                return View(member);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpGet]
        public ActionResult AdminDetails(int id)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Member member = context.Members.Include("Room").SingleOrDefault(m => m.MemberId == id);
                Room room = context.Rooms.Include("Building").Include("RoomCategory").SingleOrDefault(r => r.RoomId == member.RoomId);
                ViewBag.balance = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Debit - b.Credit);
                ViewBag.bill = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Debit);
                ViewBag.pay = context.MembersPayment.Where(p => p.MemberId == member.MemberId).Sum(b => b.Credit);
                ViewBag.Building = room.Building.BuildingName;
                ViewBag.Rent = room.RoomCategory.Rent / room.TotalSeat;
                return View(member);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }

        [HttpGet]
        public ActionResult ChangeRoom(int id)
        {
            if ((string)Session["type"] == "Manager")
            {

                int buildingid = Convert.ToInt32(Session["building"]);
                HostelDBContext context = new HostelDBContext();
                List<SelectListItem> RoomList = new List<SelectListItem>();
                SelectListItem l = new SelectListItem();
                l.Text = "Select";
                l.Value = "";
                RoomList.Add(l);
                foreach (Room room in context.Rooms.Where(r => r.BuildingId == buildingid && r.Members.Count < r.TotalSeat))
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = room.RoomNo;
                    li.Value = room.RoomId.ToString();
                    RoomList.Add(li);
                }
                ViewBag.RoomsList = RoomList;
                Member member = context.Members.SingleOrDefault(m => m.MemberId == id);
                ViewBag.Rooms = context.Rooms.ToList();
                return View(member);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost, ActionName("ChangeRoom")]
        public ActionResult RoomChange(int id)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Member member = context.Members.SingleOrDefault(m => m.MemberId == id);
                TryUpdateModel(member);
                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    TempData["Success"] = "Room Successfully Changed!";
                    return RedirectToAction("Index");
                }
                else
                {

                    return View(member);
                }
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
            List<Member> memberlist = context.Members.Where(m => m.MemberName.Contains(term) || m.Phone.Contains(term)).ToList();
            foreach (Member mem in memberlist)
            {
                name.Add(mem.MemberName);
            }

            return Json(name, JsonRequestBehavior.AllowGet);
        }

    }
}