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

        public ActionResult ExportPdf()
        {
            try
            {
                // Truy vấn dữ liệu
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

                var result = query.ToList()
                    .GroupBy(x => x.CreatedDate.Date)
                    .Select(g => new WebTH.Models.StatisticalViewModel
                    {
                        Date = g.Key.ToString("dd/MM/yyyy"),
                        DoanhThu = g.Sum(x => x.Quantity * x.Price),
                        LoiNhuan = g.Sum(x => x.Quantity * x.Price) - g.Sum(x => x.Quantity * x.OriginalPrice)
                    })
                    .OrderBy(x => DateTime.ParseExact(x.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .ToList();

                // Sử dụng StringWriter để lấy nội dung HTML từ view
                string htmlContent = "";
                using (StringWriter sw = new StringWriter())
                {
                    // Render view to string
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                        "~/Areas/Admin/Views/Statistical/_ExportStatisticalPdf.cshtml");
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View,
                        new ViewDataDictionary(result), new TempDataDictionary(), sw);
                    viewResult.View.Render(viewContext, sw);
                    htmlContent = sw.ToString();
                }

                // Tạo thư mục nếu chưa tồn tại
                string pdfDirectory = Server.MapPath("~/Resource/Pdf");
                if (!Directory.Exists(pdfDirectory))
                {
                    Directory.CreateDirectory(pdfDirectory);
                }

                // Tạo tên file
                string fileName = "ViewToPdf_" + DateTime.Now.Ticks + ".pdf";
                string pathFile = Path.Combine(pdfDirectory, fileName);

                // Tạo document với iTextSharp
                using (Document document = new Document(PageSize.A4, 50, 50, 50, 50))
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pathFile, FileMode.Create));
                    document.Open();

                    // Parse HTML (nếu HTML đơn giản có thể dùng HTMLWorker, nếu phức tạp nên dùng XMLWorker)
                    using (StringReader sr = new StringReader(htmlContent))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                    }

                    document.Close();
                }

                return File(pathFile, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, stackTrace = ex.StackTrace }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}