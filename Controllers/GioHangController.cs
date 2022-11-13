using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //Lay gio hang tra ve list item gio hang dung list
        public List<ItemGioHang> LayGioHang()
        {
            //Gio hang da ton tai dung season luu gio hang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                //Neeu sesson gio hang chua ton tai tao ra 1 list gio hang
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //Phuong thuc them gio hang load lai trang
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //Kiem tra san pham co ton tai trong CSDL khong 
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lay gio hang tạo list giỏ hàng 
            List<ItemGioHang> lstGioHang = LayGioHang();
            //TH1 san pham da ton tai trong gio hang 
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //Kiem tra so luong ton so voi so luong dat mua 
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }

            ItemGioHang itemGH = new ItemGioHang(MaSP);
            lstGioHang.Add(itemGH);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            return Redirect(strURL);
        }
        public double TinhTongSoLuong()
        {
            //Lay gio hang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }

        //Tinh tong tien
        public Decimal TinhTongTien()
        {
            //Lay gio hang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            Decimal iTongTien = 0;
            if (lstGioHang != null)
            {
                iTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return iTongTien;
        }
        // GET: GioHang
       
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = 0;
            ViewBag.TongTien = 0;
            if (TinhTongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        public ActionResult XemGioHang()
        {
            //Lấy giỏ hảng 
            List<ItemGioHang> lstGioHang = LayGioHang();

            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return View(lstGioHang);
        }
        //chỉnh sửa giỏ hàng 
      private  int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.SoLuong);
            }
            return iTongSoLuong;
        }
        [HttpGet]
        public ActionResult XoaGioHang(int MaSP)
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                lstGioHang.Remove(spCheck);
                Session["GioHang"] = lstGioHang;
            }
            return View("XemGioHang", lstGioHang);
        }
        public ActionResult CapNhapGioHang(int iMaSP , FormCollection f)
        {
            List<ItemGioHang> lstGiohang = LayGioHang();

            ItemGioHang hang = lstGiohang.SingleOrDefault(n => n.MaSP == iMaSP);
            if (hang != null)
            {
                hang.SoLuong = int.Parse(f["txtSoluong"].ToString());
                hang.ThanhTien = hang.SoLuong * hang.DonGia;
            }
            return RedirectToAction("XemGioHang");
        }
        //Xây dựng  chức năng đặt hàng 
        public ActionResult DatHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Them don hang
            DonDatHang ddh = new DonDatHang();
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //Them chi tiet don hang
            List<ItemGioHang> lstGH = LayGioHang();
            foreach(var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
    }
}