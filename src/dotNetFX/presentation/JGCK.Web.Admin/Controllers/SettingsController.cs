﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HSMY_AdminWeb.Models;
using JGCK.Framework;
using JGCK.Modules.Configuration;
using JGCK.Modules.Membership;
using JGCK.Respority.BasicInfo;
using JGCK.Util;
using JGCK.Web.Admin.Models;
using JGCK.Web.Admin.Models.Mapper;
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
        private DoctorManager m_DoctorService { get; set; }
        private UserManager m_UserService { get; set; }

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

        [ValidateInput(false)]
        public async Task<JsonResult> AddHospital(VmHospital vm)
        {
            var jsonResult = new VM_JsonOnlyResult();
            var val = new VmHospitalValidator();
            var modelState = val.Validate(vm);
            if (!modelState.IsValid)
            {
                jsonResult.Err = string.Join("<br>", modelState.Errors.Select(e => e.ErrorMessage));
                return await Task.FromResult(Json(jsonResult));
            }

            m_HospitalService.PreOnAddHandler = () => !m_HospitalService.HospitalExists(vm.NagigatedDomainObject.Name);
            var addResult = await m_HospitalService.AddObject(vm.NagigatedDomainObject, true);
            if (addResult == AppServiceExecuteStatus.Success)
            {
                jsonResult.Result = true;
                return Json(jsonResult);
            }

            jsonResult.Err = string.Format(addResult.ToDescription(), "医院名称已存在");
            return Json(jsonResult);
        }

        public async Task<JsonResult> DeleteHospital(long hId)
        {
            var jsonResult = new VM_JsonOnlyResult();
            m_HospitalService.PreLogicDeleteHandler = () =>
            {
                var hospitalExists = m_HospitalService.GetHospitalCount(m => m.ID == hId).Result > 0;
                var hospitalHasDoctor = m_DoctorService.GetDoctorCount(p =>
                    p.IsDoctor && p.Doctor.InHospital.Any(h => h.BindedHospitalId == hId)).Result > 0;
                return !hospitalExists || !hospitalHasDoctor;
            };
            var deleteResult = await m_HospitalService.LogicObjectDelete<Hospital, long>(hId);
            if (deleteResult == AppServiceExecuteStatus.Success)
            {
                jsonResult.Result = true;
                return Json(jsonResult);
            }

            jsonResult.Err = string.Format(deleteResult.ToDescription(), "医院不存在");
            return Json(jsonResult);
        }

        [ValidateInput(false)]
        public async Task<JsonResult> UpdateHospital(VmHospital vm)
        {
            var jsonResult = new VM_JsonOnlyResult();
            var val = new VmHospitalValidator();
            var modelState = val.Validate(vm);
            if (!modelState.IsValid)
            {
                jsonResult.Err = string.Join("<br>", modelState.Errors.Select(e => e.ErrorMessage));
                return await Task.FromResult(Json(jsonResult));
            }

            m_HospitalService.PreOnUpdateHandler = () =>
            {
                var ret = m_HospitalService.GetHospital(vm.NagigatedDomainObject.ID);
                var existHospital = m_HospitalService.GetHospital(vm.NagigatedDomainObject.Name);
                if (existHospital == null || existHospital.ID == vm.NagigatedDomainObject.ID)
                    return ret;
                return null;
            };
            m_HospitalService.OnUpdatingHandler = (existOject, newObject) =>
            {
                ((Hospital) newObject).MapTo((Hospital) existOject);
            };
            var updateStatus = await m_HospitalService.UpdateObject<Hospital>(vm.NagigatedDomainObject, true);
            if (updateStatus == AppServiceExecuteStatus.Success)
            {
                jsonResult.Result = true;
                return Json(jsonResult);
            }

            jsonResult.Err = string.Format(updateStatus.ToDescription(), "修改医院信息失败");
            return Json(jsonResult);
        }

        public async Task<JsonResult> GetSmartHospitals(string search)
        {
            var ret = (await m_HospitalService.GetHospitals(search)).Select(v => new VmSmartHospitalSelector
            {
                HospitalId = v.ID,
                HospitalName = v.Name
            });
            return Json(ret);
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

        [HttpPost]
        public async Task<JsonResult> AddDepartment(VmDepartment dep)
        {
            var jsonResult = new VM_JsonOnlyResult();
            var modelState = (new VmDepartmentValidator()).Validate(dep);
            if (!modelState.IsValid)
            {
                jsonResult.Err = string.Join(",", modelState.Errors.Select(m => m.ErrorMessage));
                return await Task.FromResult(Json(jsonResult));
            }

            m_DepartmentService.PreOnAddHandler =
                () => !m_DepartmentService.DepartmentExists(dep.NagigatedDomainObject.Name);
            var addedRet = await m_DepartmentService.AddObject(dep.NagigatedDomainObject, true);
            if (addedRet == AppServiceExecuteStatus.Success)
            {
                jsonResult.Value = dep.NagigatedDomainObject.ID;
                jsonResult.Result = true;
                return Json(jsonResult);
            }

            jsonResult.Err = string.Format(addedRet.ToDescription(), "部门名称已存在");
            return Json(jsonResult);
        }

        [HttpPost]
        public async Task<JsonResult> DelDepartment(long depId)
        {
            var ret = new VM_JsonOnlyResult();
            var existUserInDep = m_UserService.HasUserInDepartment(depId);
            if (existUserInDep)
            {
                ret.Err = "该部门下已存在员工信息，无法删除部门信息！";
                return Json(ret);
            }

            var deleted = await m_DepartmentService.LogicObjectDelete<Department, long>(depId, true);
            if (deleted == AppServiceExecuteStatus.Success)
            {
                ret.Value = depId;
                ret.Result = true;
                return Json(ret);
            }

            ret.Err = string.Format(deleted.ToDescription(), "部门信息不存在");
            return Json(ret);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateDepartment(VmDepartment dep)
        {
            var jsonResult = new VM_JsonOnlyResult();
            var modelState = (new VmDepartmentValidator()).Validate(dep);
            if (!modelState.IsValid)
            {
                jsonResult.Err = string.Join(",", modelState.Errors.Select(m => m.ErrorMessage));
                return await Task.FromResult(Json(jsonResult));
            }


            m_DepartmentService.PreOnUpdateHandler =
                () =>
                {
                    var existDep = m_DepartmentService.GetDepartment(dep.NagigatedDomainObject.ID);
                    if (existDep == null)
                        return null;
                    var otherDep = m_DepartmentService.GetDepartment(dep.NagigatedDomainObject.Name);
                    if (otherDep == null || otherDep.ID == dep.NagigatedDomainObject.ID)
                        return existDep;
                    return null;
                };
            m_DepartmentService.OnUpdatingHandler = (oDep, nDep) =>
            {
                ((Department)oDep).Name = ((Department)nDep).Name;
                ((Department) oDep).Desc = ((Department) nDep).Desc;
            };
            var updatedRet = await m_DepartmentService.UpdateObject(dep.NagigatedDomainObject, true);
            if (updatedRet == AppServiceExecuteStatus.Success)
            {
                jsonResult.Value = dep.NagigatedDomainObject.ID;
                jsonResult.Result = true;
            }

            jsonResult.Err = string.Format(updatedRet.ToDescription(), "部门名称重复");
            return Json(jsonResult);
        }

        #endregion
    }
}