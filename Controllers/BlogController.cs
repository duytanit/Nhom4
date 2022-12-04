using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        [ChildActionOnly]
        public ActionResult BlogPartial()
        {

            return PartialView();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}