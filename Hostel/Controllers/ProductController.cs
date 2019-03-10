using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                return View(context.Products.ToList());
            }
            else
            {
                return Redirect("~/User/Error");
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            if ((string)Session["type"] == "Manager")
            {
                return View();
            }
            else
            {
                return Redirect("~/User/Error");
            }
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                if (ModelState.IsValid)
                {
                    Product pro = context.Products.SingleOrDefault(p => p.ProductName == product.ProductName);
                    if (pro == null)
                    {
                        context.Products.Add(product);
                        context.SaveChanges();
                        TempData["Success"] = "Product Successfully Created!";
                    }
                    else
                    {
                        TempData["Failed"] = "Product already exists!";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(product);
                }
            }
            else
            {
                return Redirect("~/User/Error");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Product product = context.Products.SingleOrDefault(p => p.ProductId == id);
                return View(product);
            }
            else
            {
                return Redirect("~/User/Error");
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Update(int id)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Product product = context.Products.SingleOrDefault(p => p.ProductId == id);
                TryUpdateModel(product);
                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    TempData["Success"] = "Product Successfully Updated!";
                    return RedirectToAction("Index");


                }
                else
                {
                    return View(product);
                }
            }
            else
            {
                return Redirect("~/User/Error");
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Product product = context.Products.SingleOrDefault(p => p.ProductId == id);
                return View(product);
            }
            else
            {
                return Redirect("~/User/Error");
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Remove(int id)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Product product = context.Products.SingleOrDefault(p => p.ProductId == id);
                context.Products.Remove(product);
                context.SaveChanges();
                TempData["Success"] = "Product Successfully Deleted!";
                return RedirectToAction("Index");

            }
            else
            {
                return Redirect("~/User/Error");
            }
        }
        public JsonResult Search(string term)
        {
            HostelDBContext context = new HostelDBContext();
            List<string> name = new List<string>();
            List<Product> productlist = context.Products.Where(p => p.ProductName.Contains(term)).ToList();
            foreach (Product p in productlist)
            {
                name.Add(p.ProductName);
            }

            return Json(name, JsonRequestBehavior.AllowGet);
        }
    }
}