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
using JGCK.Web.General.Helper;
using Newtonsoft.Json;

namespace JGCK.Web.Admin.Controllers
{
    public class SettingsController : JGCK_MvcController
    {
        private WorktimeManager m_ConfigWorktimeService { get; set; }
        private DepartmentManager m_DepartmentService { get; set; }
        private HospitalManager m_HospitalService { get; set; }

        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

        #region 日历维护

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

        #endregion

        #region 医院管理

        public async Task<ActionResult> HospitalList(string filter, int? p)
        {
            var hospitalIndex = new VmHospitalIndex() { Filter = filter?.Trim() };
            var pageIndex = p.HasValue ? p.Value : 1;
            var searchExp = hospitalIndex.CombineExpression();
            var entList =
                await m_HospitalService.GetHospitalListAsync(searchExp, UserSortBy<Hospital, JsonSortValue>(ConfigHelper.KeyModuleHospitalSort),
                    pageIndex);
            hospitalIndex.TotalRecordCount = await m_HospitalService.GetHospitalCount(searchExp);
            hospitalIndex.ViewObjects = entList.Select(item => new VmHospital()
            {
                NagigatedDomainObject = item,
                ResetSettingHandler = () =>
                {
                    JsonConvert.DefaultSettings = () =>
                    {
                        var js = new JsonSerializerSettings();
                        js.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        return js;
                    };
                }
            }).ToList();
            hospitalIndex.CurrentIndex = pageIndex;
            return View(hospitalIndex);
        }

        #endregion

        #region 部门管理

        public async Task<ActionResult> DepartmentList()
        {
            var ret =  (await m_DepartmentService.GetDepartments())
                .Select(dep => new VmDepartment {NagigatedDomainObject = dep})
                .ToList();
            return View(ret);
        }

        #endregion
    }
}