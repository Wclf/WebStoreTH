using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTH.Models
{
    public class ExportReportViewModel
    {
        public List<StatisticalViewModel> Statistics { get; set; }
        public List<ProductInStockViewModel> ProductsInStock { get; set; }


    }
    public class ProductInStockViewModel
    {
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceSale { get; set; }
    }
}