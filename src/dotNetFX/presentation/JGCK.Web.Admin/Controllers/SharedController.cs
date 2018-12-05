using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JGCK.Web.General;

namespace JGCK.Web.Admin.Controllers
{
    public class SharedController : JGCK_MvcController
    {
        // GET: Shared
        public ActionResult Error(int? code = 500)
        {
            ViewBag.Code = code;
            return View();
        }
    }
}