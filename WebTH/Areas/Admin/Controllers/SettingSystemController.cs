using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTH.Models;

namespace WebTH.Areas.Admin.Controllers
{
    public class SettingSystemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/SettingSystem
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partial_Setting()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddSetting(SettingSystemViewModel req)
        {
            return View();
        }
    }
}