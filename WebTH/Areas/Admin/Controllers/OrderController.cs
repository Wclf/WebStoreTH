using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTH.Models;
using PagedList;
using Rotativa;

namespace WebTH.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]


    public class OrderController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var items = db.Orders.OrderByDescending(x => x.CreatedDate).ToList();

            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;

            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }

        public ActionResult Partial_SanPham(int id)
        {
            var items = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(items);

        }


        [HttpPost]

        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.Status = trangthai;
                db.Entry(item).Property(x => x.Status).IsModified = true;
                if(trangthai == 2)
                {
                    var items = db.OrderDetails.Where(x => x.OrderId == id);
                    foreach (var sanpham in items)
                    {
                        foreach (var dbsp in db.Products)
                        {
                            if (sanpham.ProductId == dbsp.Id)
                            {
                                dbsp.Quantity = dbsp.Quantity - sanpham.Quantity;
                            }
                        }
                    }
                }
                

                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        public ActionResult ExportInvoice(int id)
        {
            var order = db.Orders.Include("OrderDetails.Product").FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return new ViewAsPdf("ExportInvoice", order)
            {
                FileName = $"HoaDon_{order.Code}.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };
        }
    }
}