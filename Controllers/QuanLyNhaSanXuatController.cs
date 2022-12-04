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
    public class QuanLyNhaSanXuatController : Controller
    {
        // GET: QuanLyNhaSanXuat

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        public ActionResult Index(int? page, string tim)
        {
            var nsx = db.NhaSanXuats.Select(d => d);
            //hàm tìm kiếm 
           
                if (!String.IsNullOrEmpty(tim))
                {
                    nsx = nsx.Where(d => d.TenNSX.Contains(tim));
                }
           

           
            //Cần sắp xếp trước khi phân trang
            nsx = nsx.OrderBy(d => d.MaNSX);
            int pageSize = 5; //Kích thước trang
            int pageNumber = (page ?? 1); //Nếu page bằng null thì trả về 1
            return View(nsx.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChiTiet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sp = db.NhaSanXuats.FirstOrDefault(s => s.MaNSX == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }

        [HttpGet]
        public ActionResult Them()
        {
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSXs");
            return View();
        }
        [HttpPost]
        public ActionResult Them([Bind(Include = "MaNSX,TenNSX,ThongTin,Logo")] NhaSanXuat nhaSanXuat)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fhinh = Request.Files["logo"];
                    if (fhinh != null && fhinh.ContentLength > 0)
                    {
                        //lấy tên file được chọn
                        string filename = Path.GetFileName(fhinh.FileName);
                        //Save hình về thư mục HinhAnhSP trên Server
                        var pathhinh = Server.MapPath("~/Content/HinhAnhSP/" + filename);
                        fhinh.SaveAs(pathhinh);
                        nhaSanXuat.Logo = filename;
                    }
                    db.NhaSanXuats.Add(nhaSanXuat);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", nhaSanXuat.MaNSX);
                return View(nhaSanXuat);
            }
        }
        [HttpGet]
        public ActionResult Sua(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var nhasx = db.NhaSanXuats.FirstOrDefault(s => s.MaNSX == id);
            if (nhasx == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", nhasx.MaNSX);
            return View(nhasx);
        }
        [HttpPost]
        public ActionResult Sua([Bind(Include = "MaNSX,TenNSX,ThongTin,Logo")] NhaSanXuat nhaSanXuat)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fhinh = Request.Files["Logo"];
                    if (fhinh != null && fhinh.ContentLength > 0)
                    {
                        //lấy tên file được chọn
                        string filename = Path.GetFileName(fhinh.FileName);
                        //Save hình về thư mục HinhAnhSP trên Server
                        var pathhinh = Server.MapPath("~/Content/HinhAnhSP/" + filename);
                        fhinh.SaveAs(pathhinh);
                        nhaSanXuat.Logo = filename;
                    }
                    db.Entry(nhaSanXuat).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.MaNSX = new SelectList(db.NhaSanXuats, "MaNSX", "TenNSX", nhaSanXuat.MaNSX);
                return View(nhaSanXuat);
            }
        }

        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var nhasx = db.NhaSanXuats.FirstOrDefault(s => s.MaNSX == id);
            if (nhasx == null)
            {
                return HttpNotFound();
            }
            return View(nhasx);
        }

        [HttpPost]
        public ActionResult Xoa(int id, FormCollection f)
        {
            var nhasx = db.NhaSanXuats.FirstOrDefault(s => s.MaNSX == id);
            db.NhaSanXuats.Remove(nhasx);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
      
    }
}