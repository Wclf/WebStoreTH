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
            var items = db.Promotions;
            return View(items);
        }

        public ActionResult Add()
        {
            ViewBag.Products = new MultiSelectList(db.Products.ToList(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Promotion model, List<int> selectedProductIds)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebTH.Models.Common.Filter.FilterChar(model.Title);
                db.Promotions.Add(model);
                db.SaveChanges();

                /// Gán promotionId cho các sản phẩm được chọn và tính lại giá
                if (selectedProductIds != null && selectedProductIds.Count > 0)
                {
                    var products = db.Products.Where(p => selectedProductIds.Contains(p.Id)).ToList();
                    foreach (var product in products)
                    {
                        product.PromotionId = model.Id;
                        product.PriceSale = product.Price - (product.Price * model.DiscountPercent / 100);
                        product.ModifiedDate = DateTime.Now;
                        db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    }

                    db.SaveChanges(); // Lưu cập nhật
                }
                return RedirectToAction("Index");
            }
            ViewBag.Products = new MultiSelectList(db.Products.ToList(), "Id", "Title");
            return View();
        }

        public ActionResult Edit(int id)
        {
            var item = db.Promotions.Find(id);
            if (item == null) return HttpNotFound();

            // Lấy danh sách sản phẩm đang dùng khuyến mại này
            var selectedProductIds = db.Products
                                       .Where(p => p.PromotionId == item.Id)
                                       .Select(p => p.Id)
                                       .ToList();

            ViewBag.Products = new MultiSelectList(db.Products.ToList(), "Id", "Title", selectedProductIds);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Promotion model, List<int> selectedProductIds)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebTH.Models.Common.Filter.FilterChar(model.Title);
                db.Promotions.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                // 1. Xóa khuyến mại khỏi các sản phẩm cũ
                var allProducts = db.Products.Where(p => p.PromotionId == model.Id).ToList();
                foreach (var p in allProducts)
                {
                    p.PromotionId = null;
                    p.PriceSale = null;
                    db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                }

                // 2. Gán lại sản phẩm mới được chọn
                if (selectedProductIds != null && selectedProductIds.Count > 0)
                {
                    var selectedProducts = db.Products.Where(p => selectedProductIds.Contains(p.Id)).ToList();
                    foreach (var p in selectedProducts)
                    {
                        p.PromotionId = model.Id;
                        p.PriceSale = p.Price - (p.Price * model.DiscountPercent / 100);
                        db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        //xóa

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Promotions.Find(id);
            if (item != null)
            {
                // BƯỚC 1: Gỡ khuyến mại khỏi các sản phẩm liên quan
                var relatedProducts = db.Products.Where(p => p.PromotionId == item.Id).ToList();
                foreach (var product in relatedProducts)
                {
                    product.PromotionId = null;
                    product.PriceSale = null;
                    product.ModifiedDate = DateTime.Now;
                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                }

                // BƯỚC 2: Xóa khuyến mại
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