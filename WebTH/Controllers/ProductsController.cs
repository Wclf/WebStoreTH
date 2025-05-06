using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTH.Models;

namespace WebTH.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index()
        {
            // Lấy sản phẩm bán chạy
            var bestSellingProducts = db.Products
                                        .Where(p => p.ViewCount > 10) // giả sử sản phẩm bán chạy có lượt xem > 100
                                        .OrderByDescending(p => p.ViewCount)
                                        .Take(5) // lấy 5 sản phẩm bán chạy
                                        .ToList();

            // Lấy sản phẩm bán chậm
            var slowSellingProducts = db.Products
                                         .Where(p => p.ViewCount <= 10) // giả sử sản phẩm bán chậm có tồn kho > 100
                                         .OrderByDescending(p => p.ViewCount)
                                         .Take(5) // lấy 5 sản phẩm bán chậm
                                         .ToList();

            // Truyền dữ liệu vào View
            ViewBag.BestSellingProducts = bestSellingProducts;
            ViewBag.SlowSellingProducts = slowSellingProducts;

            return View();
        }

        public ActionResult Detail(string alias, int id)
        {
            var item = db.Products.Find(id);
            if(item != null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();

                // Lấy danh sách sản phẩm liên quan (cùng Category)
                var relatedProducts = db.Products
                    .Where(p => p.ProductCategoryId == item.ProductCategoryId && p.Id != item.Id)
                    .Take(5) // Lấy tối đa 5 sản phẩm liên quan
                    .ToList();

                // Truyền sản phẩm chi tiết và sản phẩm liên quan vào View
                ViewBag.RelatedProducts = relatedProducts;
            }
            return View(item);
        }

    }
}