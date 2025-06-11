using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTH.Models;
using System.Globalization;
using SelectPdf;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace WebTH.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticalController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in db.Products
                        on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };

            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate < endDate);
            }

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),

            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStatisticalByFilter(string filterType)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails on o.Id equals od.OrderId
                        join p in db.Products on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };

            // Thực hiện lấy dữ liệu trước (chuyển về danh sách trong bộ nhớ)
            var dataList = query.ToList(); // Đổi tên biến thành dataList

            // Nhóm và xử lý dữ liệu trong bộ nhớ
            var result = dataList.GroupBy(x => new
            {
                Year = x.CreatedDate.Year,
                Month = filterType == "month" || filterType == "day" ? x.CreatedDate.Month : (int?)null,
                Day = filterType == "day" ? x.CreatedDate.Day : (int?)null
            })
            .Select(group => new
            {
                Date = new DateTime(group.Key.Year, group.Key.Month ?? 1, group.Key.Day ?? 1),
                TotalBuy = group.Sum(x => x.Quantity * x.OriginalPrice),
                TotalSell = group.Sum(x => x.Quantity * x.Price),
            })
            .Select(resultItem => new // Đổi tên biến thành resultItem
            {
                Date = filterType == "day" ? resultItem.Date.ToString("dd/MM/yyyy") :
                       filterType == "month" ? resultItem.Date.ToString("MM/yyyy") :
                       resultItem.Date.ToString("yyyy"),
                DoanhThu = resultItem.TotalSell,
                LoiNhuan = resultItem.TotalSell - resultItem.TotalBuy
            })
            .ToList();

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportPdf(string selectedDate)
        {
            try
            {
                DateTime dateFilter;
                if (!DateTime.TryParseExact(selectedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFilter))
                {
                    return Content("Định dạng ngày không hợp lệ.");
                }

                // Doanh thu trong ngày
                var data = (from o in db.Orders
                            join od in db.OrderDetails on o.Id equals od.OrderId
                            join p in db.Products on od.ProductId equals p.Id
                            where DbFunctions.TruncateTime(o.CreatedDate) == dateFilter.Date
                            select new
                            {
                                o.CreatedDate,
                                od.Quantity,
                                od.Price,
                                p.OriginalPrice
                            }).ToList();

                var statistical = new List<StatisticalViewModel>();
                if (data.Any())
                {
                    var group = data.GroupBy(x => x.CreatedDate.Date).First();
                    statistical.Add(new StatisticalViewModel
                    {
                        Date = group.Key.ToString("dd/MM/yyyy"),
                        DoanhThu = group.Sum(x => x.Quantity * x.Price),
                        LoiNhuan = group.Sum(x => x.Quantity * (x.Price - x.OriginalPrice))
                    });
                }

                // Sản phẩm còn trong kho
                var productsInStock = db.Products
                    .Where(p => p.Quantity > 0)
                    .Select(p => new ProductInStockViewModel
                    {
                        Title = p.Title,
                        Quantity = p.Quantity,
                        Price = p.Price,
                        PriceSale = p.PriceSale
                    }).ToList();

                // Tạo HTML bằng Razor view
                var model = new ExportReportViewModel
                {
                    Statistics = statistical,
                    ProductsInStock = productsInStock
                };

                string htmlContent = "";
                using (StringWriter sw = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "~/Areas/Admin/Views/Statistical/_ExportStatisticalPdf.cshtml");
                    var viewContext = new ViewContext(ControllerContext, viewResult.View, new ViewDataDictionary(model), new TempDataDictionary(), sw);
                    viewResult.View.Render(viewContext, sw);
                    htmlContent = sw.ToString();
                }

                string folderPath = Server.MapPath("~/Resource/Pdf");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                string fileName = "BaoCao_" + DateTime.Now.Ticks + ".pdf";
                string filePath = Path.Combine(folderPath, fileName);

                using (Document document = new Document(PageSize.A4, 50, 50, 50, 50))
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                    document.Open();
                    using (StringReader sr = new StringReader(htmlContent))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                    }
                    document.Close();
                }

                return File(filePath, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return Content("Lỗi khi xuất PDF: " + ex.Message);
            }
        }



    }
}