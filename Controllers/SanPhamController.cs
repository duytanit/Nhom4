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
        [ChildActionOnly]
        public ActionResult SanPhamStyle3Partial()
        {

            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult CommentPartial()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult CategoriesPartial()
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

        public ActionResult single(int? maSP, int? maLSP)
        {
            //lay tat ca loai san pham
            List<LoaiSanPham> listLSP = db.LoaiSanPhams.ToList();

            ViewBag.listLSP = listLSP;

            //lay toan bo thong tin ve san pham duoc chon
            SanPham sanpham = db.SanPhams.SingleOrDefault(sp => sp.MaSP == maSP);

            ViewBag.sanpham = sanpham;

            //lay danh sach cac nha san xuat thuoc tat ca cac ma loai san pham duoc chon
            var listNSX = from nsx in db.NhaSanXuats
                          where 
                          (from sp in db.SanPhams
                           where sp.MaLoaiSP == maLSP
                           select sp.MaNSX).Contains(nsx.MaNSX)
                        select nsx;
            ViewBag.listNSX = listNSX;

            //lay ra toan bo san pham cua loai sp duoc chon
            IEnumerable<SanPham> ListSP = db.SanPhams.Where(sp => sp.MaLoaiSP == maLSP);
            ViewBag.listSP = ListSP;

            //comment


            /*IEnumerable<> comment = db.BinhLuans
                    .Join(db.ThanhViens, bl => bl.MaThanhVien, tv => tv.MaThanhVien, (bl, tv) => new { Bl = bl, Tv = tv})
                    .Where(tvbl => tvbl.Bl.MaSP == maSP);*/
            IEnumerable<BinhLuan> comment = db.BinhLuans.Where(s => s.MaSP == maSP);
            ViewBag.comment = comment;
            //day view
            return View();
        }

        public ActionResult addComment(int maSP, string textComment)
        {
            try
            {
                ThanhVien userInfo = (ThanhVien)Session["TaiKhoan"];

                BinhLuan newComment = new BinhLuan();
                newComment.MaThanhVien = userInfo.MaThanhVien;
                newComment.MaSP = maSP;
                newComment.NoiDungBL = textComment;

                db.BinhLuans.Add(newComment);
                db.SaveChanges();

                /*IQueryable<comment> query = from bl in db.BinhLuans
                            join u in db.ThanhViens on bl.MaThanhVien equals u.MaThanhVien
                            where u.MaThanhVien == newComment.MaThanhVien
                            select new comment(
                                bl.NoiDungBL,
                                u.HoTen
                            );*/
                /*IEnumerable<comment> query = db.BinhLuans
                    .Join(db.ThanhViens, bl => bl.MaThanhVien, tv => tv.MaThanhVien, (bl, tv) => new comment(tv.MaThanhVien, bl.NoiDungBL, tv.HoTen))
                    .Where(tvbl => tvbl.maThanhVien == userInfo.MaThanhVien);*/
                IEnumerable<BinhLuan> query = db.BinhLuans.Where(s => s.MaSP == maSP);
                return PartialView("CommentPartial", query);
            }
            catch(Exception ex)
            {
                return null;
            }

        }

    }

}