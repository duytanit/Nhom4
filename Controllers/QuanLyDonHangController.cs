using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using WebBanHang.Models;
using PagedList;
using System.Net;

namespace WebBanHang.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyDonHang
        public ActionResult Index(int? page)
        {
            var ddh = db.DonDatHangs.Select(d => d);
            //Cần sắp xếp trước khi phân trang
            ddh = ddh.OrderBy(d => d.MaDDH);
            int pageSize = 5; //Kích thước trang
            int pageNumber = (page ?? 1); //Nếu page bằng null thì trả về 1
            return View(ddh.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChiTiet(int id)
        {
            var ddh = db.ChiTietDonDatHangs.Where(d => d.MaDDH == id);
            return View(ddh.ToList());
        }
        public ActionResult CartConfirm(int id)
        {
            
            var cf = db.DonDatHangs.FirstOrDefault(c => c.MaDDH == id);
            cf.TinhTrangGiaoHang = true;
            UpdateModel(cf);
            db.SaveChanges();          
            return RedirectToAction("Index");
        }
    }
}