using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JGCK.Modules.Configuration;
using JGCK.Respority.BasicInfo;
using JGCK.Web.Admin.Models;
using JGCK.Web.General;
using Newtonsoft.Json;

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

        [HttpGet]
        public async Task<ActionResult> Worktime(int? year)
        {
            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            var allRestDaysInYear = await m_ConfigWorktimeService.GetOffDays(year.Value, 1, 12);
            ViewBag.RestDays = JsonConvert.SerializeObject(GetJsonRestTime(allRestDaysInYear));
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdateRestWorktime(JsonNoWorkTimes restTimes)
        {
            await m_ConfigWorktimeService.RemoveCurrentYearRestDays(restTimes.Year);
            var saveResult = await m_ConfigWorktimeService.AddNewYearRestDays(GetOfflineDays(restTimes));
            if (saveResult > 0)
            {
                return Json(new {result = 1});
            }

            return Json(new {result = 0});
        }

        private JsonNoWorkTimes GetJsonRestTime(IEnumerable<OffDay> offDays)
        {
            var ret = new JsonNoWorkTimes();
            foreach (var groupDaysInYear in offDays.GroupBy(od => od.NonworkDate.Value.Year))
            {
                ret.Year = groupDaysInYear.Key;
                ret.RestMonths = new List<JsonNoWorkDaysInMonth>();
                foreach (var groupDaysInMonth in groupDaysInYear.GroupBy(m => m.NonworkDate.Value.Month))
                {
                    var daysInMonth = new JsonNoWorkDaysInMonth();
                    daysInMonth.Month = groupDaysInMonth.Key;
                    daysInMonth.Days = groupDaysInMonth.Select(od => od.NonworkDate.Value.Day).ToList();
                    ret.RestMonths.Add(daysInMonth);
                }
            }

            return ret;
        }

        private IEnumerable<OffDay> GetOfflineDays(JsonNoWorkTimes restTimes)
        {
            var ret = new List<OffDay>();
            var lstYearMonthDays = restTimes.RestMonths
                .Select(m => new
                {
                    strYearAndMonth = restTimes.Year.ToString() + "-" + m.Month,
                    days = m.Days
                })
                .ToList();
            lstYearMonthDays.ForEach(t =>
            {
                ret.AddRange(t.days.Select(d =>
                    new OffDay
                    {
                        CreateDate = DateTime.Now,
                        NonworkDate = Convert.ToDateTime(t.strYearAndMonth + "-" + d)
                    }
                ));
            });
            return ret;
        }
    }
}