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
    public class QuanLyNhaCungCapController : Controller
    {
        // GET: QuanLyNhaCungCap

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        public ActionResult Index(int? page, string tim)
        {
            var ncc = db.NhaCungCaps.Select(d => d);

            if (!String.IsNullOrEmpty(tim))
            {
                ncc = ncc.Where(d => d.TenNCC.Contains(tim));
            }
            //Cần sắp xếp trước khi phân trang
            ncc = ncc.OrderBy(d => d.MaNCC);
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
            var ncc = db.NhaCungCaps.FirstOrDefault(s => s.MaNCC == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }
            return View(ncc);
        }

        [HttpGet]
        public ActionResult Sua(int? id)
        {
          
            var ncc = db.NhaCungCaps.FirstOrDefault(s => s.MaNCC == id);
            return View(ncc);
        }

        [HttpPost]
        public ActionResult Sua(int? id, FormCollection data)
        {
            var ncc = db.NhaCungCaps.FirstOrDefault(s => s.MaNCC == id);
            var ten = data["TenNCC"];
            var dc = data["DiaChi"];
            var email = data["Email"];
            var sdt = data["SoDienThoai"];
            var fax = data["Fax"];
            if(string.IsNullOrEmpty(ten))
            {
                ViewData["loi1"] = "Tên Nhà Cung Cấp Không Được Để Trống";
            }
            else
                if (string.IsNullOrEmpty(dc))
            {
                ViewData["loi2"] = "Địa Chỉ Không Được Để Trống";
            }
            else
                if (string.IsNullOrEmpty(email))
            {
                ViewData["loi3"] = "Email Không Được Để Trống";
            }
            else
                if (string.IsNullOrEmpty(sdt))
            {
                ViewData["loi4"] = "Số Điện Thoại Không Được Để Trống";
            }
            else
                if (string.IsNullOrEmpty(fax))
            {
                ViewData["loi5"] = "Số Fax Không Được Để Trống";
            }
            else
            {
                ncc.TenNCC = ten;
                ncc.DiaChi = dc;
                ncc.Email = email;
                ncc.SoDienThoai = sdt;
                ncc.Fax = fax;
                UpdateModel(ncc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return this.Sua(id);
        }

        [HttpGet]
        public ActionResult Them()
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            return View();
        }
        [HttpPost]
        public ActionResult Them([Bind(Include = "MaNCC,TenNCC,DiaChi,Email,SoDienThoai,Fax")] NhaCungCap nhaCungCap)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.NhaCungCaps.Add(nhaCungCap);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNSX", "TenNSX",nhaCungCap.MaNCC);
                return View(nhaCungCap);
            }
        }

        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ncc = db.NhaCungCaps.FirstOrDefault(s => s.MaNCC == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }
            return View(ncc);
        }

        [HttpPost]
        public ActionResult Xoa(int id, FormCollection f)
        {
            var ncc = db.NhaCungCaps.FirstOrDefault(s => s.MaNCC == id);
            db.NhaCungCaps.Remove(ncc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }

}
