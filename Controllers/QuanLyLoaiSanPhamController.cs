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
    public class QuanLyLoaiSanPhamController : Controller
    {
        // GET: QuanLyLoaiSanPham

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index(int? page, string tim)
        {
            var ncc = db.LoaiSanPhams.Select(d => d);

            if (!String.IsNullOrEmpty(tim))
            {
                ncc = ncc.Where(d => d.TenLoai.Contains(tim));
            }
            //Cần sắp xếp trước khi phân trang
            ncc = ncc.OrderBy(d => d.MaLoaiSP);
            int pageSize = 5; //Kích thước trang
            int pageNumber = (page ?? 1); //Nếu page bằng null thì trả về 1
            return View(ncc.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChiTiet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ncc = db.LoaiSanPhams.FirstOrDefault(s => s.MaLoaiSP == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }
            return View(ncc);
        }
        [HttpGet]
        public ActionResult Them()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai");
            return View();
        }
        [HttpPost]
        public ActionResult Them([Bind(Include = "MaLoaiSP,TenLoai,BiDanh")] LoaiSanPham loaiSanPham)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    db.LoaiSanPhams.Add(loaiSanPham);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoai", loaiSanPham.MaLoaiSP);
                return View(loaiSanPham);
            }
        }

        [HttpGet]
        public ActionResult Sua(int? id)
        {

            var ncc = db.LoaiSanPhams.FirstOrDefault(s => s.MaLoaiSP == id);
            return View(ncc);
        }

        [HttpPost]
        public ActionResult Sua(int? id, FormCollection data)
        {
            var lsp = db.LoaiSanPhams.FirstOrDefault(s => s.MaLoaiSP == id);
            var ten = data["TenLoai"];
            var bidanh = data["BiDanh"];
            if (string.IsNullOrEmpty(ten))
            {
                ViewData["loi1"] = "Tên Loại sản Phẩm  Không Được Để Trống";
            }
            else
                if (string.IsNullOrEmpty(bidanh))
            {
                ViewData["loi2"] = "BiDanh Không Được Để Trống";
            }
            else
            {
                lsp.TenLoai = ten;
                lsp.BiDanh = bidanh;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return this.Sua(id);
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lsp = db.LoaiSanPhams.FirstOrDefault(s => s.MaLoaiSP == id);
            if (lsp == null)
            {
                return HttpNotFound();
            }
            return View(lsp);
        }

        [HttpPost]
        public ActionResult Xoa(int id, FormCollection f)
        {
            var lsp = db.LoaiSanPhams.FirstOrDefault(s => s.MaLoaiSP == id);
            db.LoaiSanPhams.Remove(lsp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}