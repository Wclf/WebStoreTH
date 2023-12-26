using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTH.Models;

namespace WebTH.Controllers
{
    public class ProductListController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProductList
        public ActionResult Index()
        {
            var items = db.Products.ToList();
            return View(items);
        }

        public ActionResult Partial_ItemByCateId()
        {
            var items = db.Products.Where(x => x.IsHome && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }


    }
}