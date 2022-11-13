using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using PagedList;
namespace WebBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //tạo 2 partial view sản phẩm 1 và 2 để hiển thị sản phẩm theo 2 style khác nhau 
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {

            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {

            return PartialView();
        }
        //Xay dụng 1 action load sản phẩm theo mã loại sản phẩm  và mã nhà sản xuất (int)
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX , int? page)
        {
            //khong che k cho ko dang nhap ma truy cap vao san pham
            //if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            if (MaLoaiSP == null && MaNSX == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP.Count() == 0)
            {
                return HttpNotFound();
            }
            //Thuc hien  phan trang 
            //Tao bien so  san pham tren trang 
            int pageSize = 6;
            //Tao bien thu 2 : so trang hien tai 
            int PageNumber =(page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX; 
            return View(lstSP.OrderBy(n=>n.MaSP).ToPagedList(PageNumber,pageSize));
        }
        public ActionResult single()
        {

            return View();
        }

    }

}