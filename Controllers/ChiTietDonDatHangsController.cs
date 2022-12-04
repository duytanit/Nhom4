using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class ChiTietDonDatHangsController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: ChiTietDonDatHangs
        public ActionResult Index()
        {
            var chiTietDonDatHangs = db.ChiTietDonDatHangs.Include(c => c.DonDatHang).Include(c => c.SanPham);
            return View(chiTietDonDatHangs.ToList());
        }

        // GET: ChiTietDonDatHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonDatHang chiTietDonDatHang = db.ChiTietDonDatHangs.Find(id);
            if (chiTietDonDatHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDonDatHang);
        }

        // GET: ChiTietDonDatHangs/Create
        public ActionResult Create()
        {
            ViewBag.MaDDH = new SelectList(db.DonDatHangs, "MaDDH", "MaDDH");
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP");
            return View();
        }

        // POST: ChiTietDonDatHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaChiTietDDH,MaDDH,MaSP,TenSP,SoLuong,DonGia")] ChiTietDonDatHang chiTietDonDatHang)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietDonDatHangs.Add(chiTietDonDatHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDDH = new SelectList(db.DonDatHangs, "MaDDH", "MaDDH", chiTietDonDatHang.MaDDH);
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", chiTietDonDatHang.MaSP);
            return View(chiTietDonDatHang);
        }

        // GET: ChiTietDonDatHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonDatHang chiTietDonDatHang = db.ChiTietDonDatHangs.Find(id);
            if (chiTietDonDatHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDDH = new SelectList(db.DonDatHangs, "MaDDH", "MaDDH", chiTietDonDatHang.MaDDH);
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", chiTietDonDatHang.MaSP);
            return View(chiTietDonDatHang);
        }

        // POST: ChiTietDonDatHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaChiTietDDH,MaDDH,MaSP,TenSP,SoLuong,DonGia")] ChiTietDonDatHang chiTietDonDatHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietDonDatHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDDH = new SelectList(db.DonDatHangs, "MaDDH", "MaDDH", chiTietDonDatHang.MaDDH);
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", chiTietDonDatHang.MaSP);
            return View(chiTietDonDatHang);
        }

        // GET: ChiTietDonDatHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonDatHang chiTietDonDatHang = db.ChiTietDonDatHangs.Find(id);
            if (chiTietDonDatHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDonDatHang);
        }

        // POST: ChiTietDonDatHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietDonDatHang chiTietDonDatHang = db.ChiTietDonDatHangs.Find(id);
            db.ChiTietDonDatHangs.Remove(chiTietDonDatHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
