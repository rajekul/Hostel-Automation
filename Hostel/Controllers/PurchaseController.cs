using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hostel.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: Purchase
        public ActionResult Index()
        {
            ViewBag.TotalAmount = 0;
            if ((string)Session["type"] == "Manager")
            {
                int id = Convert.ToInt32(Session["building"]);
                HostelDBContext context = new HostelDBContext();
                try
                {
                    ViewBag.TotalAmount = context.Purchases.Where(p => p.BuildingId == id).Sum(p => p.Price);
                }
                catch { }
                return View(context.Purchases.Include("Product").Where(p=>p.BuildingId==id).ToList());
            }
            else
            {
                return Redirect("~/User/Error");
            }
        }

        public ActionResult Searched(string id,string op="")
        {
            ViewBag.TotalAmount = 0;
            if ((string)Session["type"] == "Manager")
            {
                int buildingid = Convert.ToInt32(Session["building"]);
                HostelDBContext context = new HostelDBContext();
                if (op == "")
                {
                    try
                    {
                        ViewBag.TotalAmount = context.Purchases.Where(p => p.Product.ProductName == id && p.BuildingId == buildingid).Sum(p => p.Price);
                    }
                    catch { }
                    return View(context.Purchases.Include("Product").Where(p => p.Product.ProductName == id && p.BuildingId == buildingid).ToList());
                }
                else
                {
                    DateTime dateto = Convert.ToDateTime(op);
                    DateTime datefrom = Convert.ToDateTime(id);
                    try
                    {
                        ViewBag.TotalAmount = context.Purchases.Where(p => p.PurchaseDate >= datefrom && p.PurchaseDate <= dateto && p.BuildingId == buildingid).Sum(p => p.Price);
                    }catch { }
                    return View(context.Purchases.Include("Product").Where(p =>p.PurchaseDate>=datefrom && p.PurchaseDate<=dateto && p.BuildingId == buildingid).ToList());
                }
            }
            else
            {
                return Redirect("~/User/Error");
            }
        }

        public ActionResult AdminView(int id)
        {
            ViewBag.TotalAmount = 0;
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                try
                {
                    ViewBag.TotalAmount = context.Purchases.Where(p => p.BuildingId == id).Sum(p => p.Price);
                }
                catch { }
                ViewBag.buildingid = id;
                return View(context.Purchases.Include("Product").Where(p => p.BuildingId == id).ToList());
            }
            else
            {
                return Redirect("~/User/Error");
            }
        }

        public ActionResult AdminSearched(int id,string op, string oop = "")
        {
            ViewBag.TotalAmount = 0;
            if ((string)Session["type"] == "Admin")
            {
                HostelDBContext context = new HostelDBContext();
                ViewBag.buildingid = id;
                if (oop == "")
                {
                    try
                    {
                        ViewBag.TotalAmount = context.Purchases.Where(p => p.Product.ProductName == op && p.BuildingId == id).Sum(p => p.Price);
                    }
                    catch { }
                    return View(context.Purchases.Include("Product").Where(p => p.Product.ProductName == oop && p.BuildingId == id).ToList());
                }
                else
                {
                    DateTime dateto = Convert.ToDateTime(oop);
                    DateTime datefrom = Convert.ToDateTime(op);
                    try
                    {
                        ViewBag.TotalAmount = context.Purchases.Where(p => p.PurchaseDate >= datefrom && p.PurchaseDate <= dateto && p.BuildingId == id).Sum(p => p.Price);
                    }
                    catch { }
                    return View(context.Purchases.Include("Product").Where(p => p.PurchaseDate >= datefrom && p.PurchaseDate <= dateto && p.BuildingId == id).ToList());
                }
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
                HostelDBContext context = new HostelDBContext();
                List<SelectListItem> ProductList = new List<SelectListItem>();
                foreach (Product p in context.Products)
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = p.ProductName+" ("+p.Unit+")";
                    li.Value = p.ProductId.ToString();
                    ProductList.Add(li);
                }

                ViewBag.Products = ProductList;
                return View();
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost]
        public ActionResult Create(Purchase purchase)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                purchase.PurchaseDate = DateTime.Now;
                purchase.BuildingId= Convert.ToInt32(Session["building"]);
                if (ModelState.IsValid)
                {
                    
                    context.Purchases.Add(purchase);
                    context.SaveChanges();
                    TempData["Success"] = "Product Purchase Added!";
                    return RedirectToAction("Index");
                }
                else
                {
                    List<SelectListItem> ProductList = new List<SelectListItem>();
                    foreach (Product p in context.Products)
                    {
                        SelectListItem li = new SelectListItem();
                        li.Text = p.ProductName + " (" + p.Unit + ")";
                        li.Value = p.ProductId.ToString();
                        ProductList.Add(li);
                    }

                    ViewBag.Products = ProductList;
                    return View(purchase);
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
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Purchase purchase= context.Purchases.SingleOrDefault(p => p.PurchaseId == id);
                List<SelectListItem> ProductList = new List<SelectListItem>();
                foreach (Product p in context.Products)
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = p.ProductName + " (" + p.Unit + ")";
                    li.Value = p.ProductId.ToString();
                    ProductList.Add(li);
                }

                ViewBag.Products = ProductList;
                return View(purchase);
            }
            else
            {
                return Redirect("/User/Error");
            }
        }
        [HttpPost,ActionName("Edit")]
        public ActionResult Update(int id)
        {
            if ((string)Session["type"] == "Manager")
            {
                HostelDBContext context = new HostelDBContext();
                Purchase purchase = context.Purchases.SingleOrDefault(p => p.PurchaseId == id);
                TryUpdateModel(purchase);
                if (ModelState.IsValid)
                {
                    context.SaveChanges();
                    TempData["Success"] = "Product Purchase Updated!";
                    return RedirectToAction("Index");
                }
                else
                {
                    List<SelectListItem> ProductList = new List<SelectListItem>();
                    foreach (Product p in context.Products)
                    {
                        SelectListItem li = new SelectListItem();
                        li.Text = p.ProductName + " (" + p.Unit + ")";
                        li.Value = p.ProductId.ToString();
                        ProductList.Add(li);
                    }

                    ViewBag.Products = ProductList;
                    return View(purchase);
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
                Purchase purchase = context.Purchases.Include("Product").SingleOrDefault(p => p.PurchaseId == id);
                return View(purchase);
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
                Purchase purchase = context.Purchases.SingleOrDefault(p => p.PurchaseId == id);
                context.Purchases.Remove(purchase);
                context.SaveChanges();
                TempData["Success"] = "Product Purchase Deleted!";
                return RedirectToAction("Index");


            }
            else
            {
                return Redirect("/User/Error");
            }
        }
    }
}