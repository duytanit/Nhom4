using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using CaptchaMvc;
using CaptchaMvc.HtmlHelpers;

namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //private List<SanPham> LaySPMoi(int count)
        //{
        //    return db.SanPhams.OrderByDescending(a => a.NgayCapNhap).Take(count).ToList();
        //}
        public ActionResult Index()
        {
            //var sachmoi = LaySPMoi(2);
            //tao viewbag de lay list san pham tu csdl
            //List dien thoai moi nhat
            var lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false);
            //ga vao viewbag truyen list thong qua view
            ViewBag.ListDTM = lstDTM;
            var lstLTM = db.SanPhams.Where(n => n.MaLoaiSP == 10 && n.Moi == 1 && n.DaXoa == false);
            //ga vao viewbag truyen list thong qua view
            ViewBag.ListLTM = lstLTM;
            var lstMTB = db.SanPhams.Where(n => n.MaLoaiSP == 9 && n.Moi == 1 && n.DaXoa == false);
            //ga vao viewbag truyen list thong qua view
            ViewBag.ListMTB = lstMTB;
            return View();
        }

      

        public ActionResult ChiTietSanPham(int id)
        {
            //truy vấn về 1 list sản phẩm
            var sanpham = from sp in db.SanPhams
                          where sp.MaSP == id
                          select sp;

            return View(sanpham.Single());
        }
        public ActionResult MenuPartial()
        {
            //truy vấn về 1 list sản phẩm
            var lstSP = db.SanPhams;

            return PartialView(lstSP);
        }
        public ActionResult MenuPartial1()
        {
            //truy vấn về 1 list sản phẩm
            var lstSP = db.SanPhams;

            return PartialView(lstSP);
        }
        [HttpGet]
        public ActionResult DangKy()
        {


            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            //Kiem tra captcha hop le
            if (this.IsCaptchaValid("captcha is not vaild "))
            {

                ViewBag.ThongBao = "Thêm thành công ";
                //Them thanh vien vao csdl
                db.ThanhViens.Add(tv);
                db.SaveChanges();
                return View();
            }
            ViewBag.ThongBao = "Sai mã captcha ";
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            // taif khoan ko trung nhau
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null)
            {
                Session["TaiKhoan"] = tv;
                return Content("<script>window.location.reload();</script>");
            }
            return Content("Tai khoan mat khau khong chinh xac");
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index");
        }
    }
}