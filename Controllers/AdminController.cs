using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            //return View();
            return View(db.SanPhams.Where(n=>n.DaXoa==false));
        }
        public ActionResult Dtreport(FormCollection f)
        {
            var fdate = f["fromdate"];
            var tdate = f["tdate"];
            ViewBag.fdate = Request["fromdate"];
            ViewBag.tdate = Request["todate"];
            var ddh = db.ChiTietDonDatHangs.Include(d => d.DonDatHang).Include(d => d.SanPham)
                .Where(d=>d.DonDatHang.NgayDat.Value.Date > Convert.ToDateTime(fdate) && d.DonDatHang.NgayDat < Convert.ToDateTime(tdate));
            return View(ddh.ToList());
        }
        public ActionResult SanPhamPartial()
        {
            var sp = db.SanPhams;
            return PartialView(sp);
        }
    }
}