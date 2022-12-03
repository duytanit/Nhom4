using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using PagedList;
using System.IO;
using System.Data.Entity;
using System.Net;

namespace WebBanHang.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        // GET: QuanLySanPham
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index(int? page)
        {
            var sp = db.SanPhams.Where(s => s.DaXoa == false);
            //Cần sắp xếp trước khi phân trang
            sp = sp.OrderBy(s => s.MaSP);
            int pageSize = 9; //Kích thước trang
            int pageNumber = (page ?? 1); //Nếu page bằng null thì trả về 1
            return View(sp.ToPagedList(pageNumber, pageSize));
        }      

        public ActionResult ChiTiet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sp = db.SanPhams.FirstOrDefault(s => s.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        [HttpGet]
        public ActionResult Them()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX");
            return View();
        }
        [HttpPost]
        public ActionResult Them([Bind(Include = "MaSP,TenSP,DonGia,NgayCapNhap,CauHinh,MoTa,SoLuongTon,LuotXem,LuotBinhChon,LuotBinhLuan,SoLanMua,Moi,MaNCC,MaNSX,MaLoaiSP,DaXoa")] SanPham sanPham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fhinh = Request.Files["hinhanh"];
                    if(fhinh != null && fhinh.ContentLength > 0)
                    {
                        //lấy tên file được chọn
                        string filename = Path.GetFileName(fhinh.FileName);
                        //Save hình về thư mục HinhAnhSP trên Server
                        var pathhinh = Server.MapPath("~/Content/HinhAnhSP/" + filename);
                        fhinh.SaveAs(pathhinh);
                        sanPham.HinhAnh = filename;
                    }                  
                    db.SanPhams.Add(sanPham);
                    db.SaveChanges();                    
                }
                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai", sanPham.MaLoaiSP);
                ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
                ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sanPham.MaNSX);
                return View(sanPham);
            }
        }

        [HttpGet]
        public ActionResult Sua(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sanPham = db.SanPhams.FirstOrDefault(s => s.MaSP == id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai", sanPham.MaLoaiSP);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sanPham.MaNSX);
            return View(sanPham);
        }

        [HttpPost]
        public ActionResult Sua([Bind(Include = "MaSP,TenSP,DonGia,NgayCapNhap,CauHinh,MoTa,HinhAnh,SoLuongTon,LuotXem,LuotBinhChon,LuotBinhLuan,SoLanMua,Moi,MaNCC,MaNSX,MaLoaiSP,DaXoa")] SanPham sanPham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fhinh = Request.Files["hinhanh"];
                    if (fhinh != null && fhinh.ContentLength > 0)
                    {
                        //lấy tên file được chọn
                        string filename = Path.GetFileName(fhinh.FileName);
                        //Save hình về thư mục HinhAnhSP trên Server
                        var pathhinh = Server.MapPath("~/Content/HinhAnhSP/" + filename);
                        fhinh.SaveAs(pathhinh);
                        sanPham.HinhAnh = filename;
                    }
                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai", sanPham.MaLoaiSP);
                ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
                ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", sanPham.MaNSX);
                return View(sanPham);
            }          
        }

        public ActionResult RecycleBin(int? page)
        {
            var sp = db.SanPhams.Where(s => s.DaXoa == true);
            //Cần sắp xếp trước khi phân trang
            sp = sp.OrderBy(s => s.MaSP);
            int pageSize = 9; //Kích thước trang
            int pageNumber = (page ?? 1); //Nếu page bằng null thì trả về 1
            return View(sp.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sanPham = db.SanPhams.FirstOrDefault(s => s.MaSP == id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        [HttpPost]
        public ActionResult Xoa(int id, FormCollection f)
        {
            var sanPham = db.SanPhams.FirstOrDefault(s => s.MaSP == id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("RecycleBin");
        }

        

        public ActionResult XoaMem(int id)
        {
            var sp = db.SanPhams.FirstOrDefault(s => s.MaSP == id);
            sp.DaXoa = true;
            UpdateModel(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult KhoiPhuc(int id)
        {
            var sp = db.SanPhams.FirstOrDefault(s => s.MaSP == id);
            sp.DaXoa = false;
            UpdateModel(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}