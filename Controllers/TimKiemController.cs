using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

using PagedList;

namespace WebBanHang.Controllers

{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa,int? page)
        {
            if(Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            //Tim kiem theo ten
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(n=>n.TenSP).ToPagedList(pageNumber,pageSize));
        }
        [HttpPost]
        public ActionResult LayKQTimKiem(string sTuKhoa)
        {
           
            return RedirectToAction("KQTimKiem",new {@sTuKhoa= sTuKhoa });
        }
    }
}