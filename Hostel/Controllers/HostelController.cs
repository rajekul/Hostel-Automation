using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class HostelController : Controller
    {
        // GET: Hostel
        public ActionResult Index()
        {
            if ((string)Session["type"]=="Admin") {
                HostelDBContext context = new HostelDBContext();
                Hostel hostel = context.Hostels.SingleOrDefault(h => h.HostelId == 1);
                return View(hostel);
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
                Hostel hostel = context.Hostels.SingleOrDefault(h => h.HostelId == id);
                return View(hostel);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost,ActionName("Edit")]
        public ActionResult Update(int id, HttpPostedFileBase file)
        {
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                Hostel hostel= context.Hostels.SingleOrDefault(h => h.HostelId == id);

                try
                {

                    if (Request.Files.Count > 0)
                    {
                        var Inputfile = Request.Files[0];

                        if (Inputfile != null && Inputfile.ContentLength > 0)
                        {
                            if (System.IO.File.Exists("~/Uploads/Logos"+hostel.HostelLogo))
                            {
                                System.IO.File.Delete("~/Uploads/Logos" + hostel.HostelLogo);
                            }
                            var filename = Path.GetFileName(Inputfile.FileName);
                            var path = Path.Combine(Server.MapPath("~/Uploads/Logos"), "hostellogo" + Path.GetExtension(Inputfile.FileName));
                            Inputfile.SaveAs(path);
                            hostel.HostelLogo = "hostellogo" + Path.GetExtension(Inputfile.FileName);
                        }

                    }

                }
                catch (Exception)
                {

                    TempData["Failed"] = "Failed To Upload Logo";

                }

                TryUpdateModel(hostel);

                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    Session["HostelTitle"] = hostel.HostelTitle;
                    TempData["Success"] = "Hostel Info Successfully Updated!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(hostel);
                }
                
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        
    }
}