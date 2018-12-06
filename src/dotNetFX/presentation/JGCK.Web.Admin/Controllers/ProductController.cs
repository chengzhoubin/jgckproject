using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JGCK.Web.General;

namespace JGCK.Web.Admin.Controllers
{
    public class ProductController : JGCK_MvcController
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
    }
}