using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
namespace WebBanHang.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            //truy vấn dữ liệu thông qua câu lệnh
            //đối  listKH sẽ lấy toàn bộ dữ từ bảng  liệu bảng KH trả về view là 1 list các khách hàng
            //cách 1 
            // var listKH = from KH in db.KhachHangs select KH;
            // return View(listKH);
            //Cách 2 dung phuong thuc ho tro san
            var lstKH = db.KhachHangs;
            return View(lstKH);
        }
        public ActionResult Index1()
        {
            //truy vấn dữ liệu thông qua câu lệnh
            //đối  listKH sẽ lấy toàn bộ dữ từ bảng  liệu bảng KH trả về view là 1 list các khách hàng
            var listKH = from KH in db.KhachHangs select KH;
            return View(listKH);
        }
        public ActionResult TruyVanDoiTuong()
        {
            // truy van 1 doi tuong lay tt khanh hang
            var lstKH = from kh in db.KhachHangs where kh.MaKH == 1 select kh;
            //Buoc 2 lay ra doi tuong khach hang bang lisst
            //KhachHang khang = lstKH.FirstOrDefault();
            //Cach 2 nen dung
            KhachHang khang = db.KhachHangs.SingleOrDefault(n => n.MaKH == 1);
            return View(khang);
        }
        public ActionResult SortDuLieu()
        {
            //xap xep
            List<KhachHang> lstKH = db.KhachHangs.OrderBy(n => n.TenKH).ToList();
            return View(lstKH);
        }
        public ActionResult GroupDuLieu()
        {
            //xap xep
            List<ThanhVien> lstKH = db.ThanhViens.OrderBy(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }
        public ActionResult ht()
        {
            //xap xep

            return View();
        }


    }
}