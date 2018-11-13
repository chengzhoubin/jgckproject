using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JGCK.Web.General;

namespace JGCK.Web.Admin.Controllers
{
    public class SettingsController : JGCK_MvcController
    {
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }
    }
}