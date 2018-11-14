using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JGCK.Modules.Configuration;
using JGCK.Web.General;

namespace JGCK.Web.Admin.Controllers
{
    public class SettingsController : JGCK_MvcController
    {
        private WorktimeManager m_ConfigWorktimeService { get; set; }

        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Worktime(int? year)
        {
            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            var allRestDaysInYear = await m_ConfigWorktimeService.GetOffDays(year.Value, 1, 12);
            return View();
        }

        public async Task<ActionResult> AddOrUpdateRestWorktime()
        {
            return View("Worktime");
        }
    }
}