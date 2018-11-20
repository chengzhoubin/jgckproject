using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JGCK.Web.General;

namespace JGCK.Web.Admin.Controllers
{
    public class PermissionController : JGCK_MvcController
    {
        // GET: Permission
        public ActionResult Index()
        {
            return View();
        }
    }
}