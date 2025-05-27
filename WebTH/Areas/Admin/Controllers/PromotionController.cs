using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTH.Models;
using WebTH.Models.EF;

namespace WebTH.Areas.Admin.Controllers
{
    public class PromotionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Promotion
        public ActionResult Index()
        {
            var items = db.Promotions;
            return View(items);
        }

        public ActionResult Hienthi()
        {
            var items = db.ProductCategories;
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Promotion model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebTH.Models.Common.Filter.FilterChar(model.Title);
                db.Promotions.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        //xóa

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Promotions.Find(id);
            if (item != null)
            {
                db.Promotions.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}