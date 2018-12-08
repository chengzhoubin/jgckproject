using HSMY_AdminWeb.Models;
using JGCK.Framework;
using JGCK.Modules.Configuration;
using JGCK.Modules.Membership;
using JGCK.Respority.UserWork;
using JGCK.Util;
using JGCK.Web.Admin.Models;
using JGCK.Web.Admin.Models.Mapper;
using JGCK.Web.General;
using JGCK.Web.General.Helper;
using JGCK.Web.General.MVC;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JGCK.Web.Admin.Controllers
{
    public class UserController : JGCK_MvcController
    {
        private UserManager m_UserManagerService { get; set; }
        private DoctorManager m_DoctorManagerService { get; set; }
        private DepartmentManager m_DepartmentManagerService { get; set; }
        private RoleManager m_RoleManagerService { get; set; }
        private HospitalManager m_HospitalManagerService { get; set; }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(VmUserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return View(userLogin);
            }

            var ret = await m_UserManagerService.CheckAsync(userLogin.UserName, userLogin.Pwd);
            if (ret == CheckUserPwdResult.Success)
            {
                var token = new JGCKUserToken
                {
                    UserName = userLogin.UserName,
                    RoleName = "Admin"
                };
                token.BuildToken();
                return RedirectToAction("Index", "Settings");
            }

            ModelState.AddModelError("UserPwdMatch", "用户名和密码不匹配");
            return View(userLogin);
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        #region 医生信息管理

        [HttpGet]
        public async Task<ActionResult> DoctorList(string filter, int? p)
        {
            var doctorIndex = new VmUserDoctorIndex() {Filter = filter?.Trim()};
            var pageIndex = p.HasValue ? p.Value : 1;
            var searchExp = doctorIndex.CombineExpression();
            var entList =
                await m_DoctorManagerService.GetDoctorListAsync(searchExp,
                    UserSortBy<Person, JsonSortValue>(ConfigHelper.KeyModuleDoctorSort),
                    pageIndex);
            doctorIndex.TotalRecordCount = await m_DoctorManagerService.GetDoctorCount(searchExp);
            doctorIndex.ViewObjects = entList.Select(item => new VmUserDoctor
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
            doctorIndex.CurrentIndex = pageIndex;
            return View(doctorIndex);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateDoctor(VmUserDoctor doctor)
        {
            var jsonResult = new VM_JsonOnlyResult();
            var val = new VmDoctorValidator();
            var modelState = val.Validate(doctor);
            if (!modelState.IsValid)
            {
                jsonResult.Err = string.Join(",", modelState.Errors.Select(e => e.ErrorMessage));
                return await Task.FromResult(Json(jsonResult));
            }

            m_DoctorManagerService.PreOnUpdateHandler =
                () => m_DoctorManagerService.GetDoctor(doctor.NagigatedDomainObject.ID);
            m_DoctorManagerService.OnUpdatingHandler = (o, n) => { VmDoctorMapper.MapTo(((Person) n), (Person) o); };
            var updateStatus = await m_DoctorManagerService.UpdateObject(doctor.NagigatedDomainObject);
            if (updateStatus == AppServiceExecuteStatus.Success)
            {
                jsonResult.Result = true;
                return Json(jsonResult);
            }

            jsonResult.Err = string.Format(updateStatus.ToDescription(), "更新医生信息失败");
            return Json(jsonResult);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteDoctor(long doctorId)
        {
            var jsonResult = new VM_JsonOnlyResult();
            var selectedDoctor = m_DoctorManagerService.GetDoctor(doctorId);
            var parentPersonId = selectedDoctor?.Doctor.WithPerson.ID ?? 0L;
            m_DoctorManagerService.PreLogicDeleteHandler = () =>
            {
                if (selectedDoctor != null && selectedDoctor.Doctor.AuditStatus == DoctorAuditStatus.Pending)
                {
                    parentPersonId = selectedDoctor.ID;
                    return true;
                }

                return false;
            };
            var deleteStatus = await m_DoctorManagerService.LogicObjectDelete<Person, long>(parentPersonId);
            if (deleteStatus == AppServiceExecuteStatus.Success)
            {
                jsonResult.Result = true;
                return Json(jsonResult);
            }

            jsonResult.Err = string.Format(deleteStatus.ToDescription(), "当前审核状态不是待审核");
            return Json(jsonResult);
        }

        [HttpPost]
        public async Task<JsonResult> AuditDoctor(JsonDoctorAudit audit)
        {
            var jsonResult = new VM_JsonOnlyResult();
            var modelState = (new JsonDoctorAuditValidator()).Validate(audit);
            if (!modelState.IsValid)
            {
                jsonResult.Value = -1001;
                jsonResult.Err = string.Join(",", modelState.Errors.Select(m => m.ErrorMessage));
                return await Task.FromResult(Json(jsonResult));
            }

            m_DoctorManagerService.PreOnUpdateHandler = () => m_DoctorManagerService.GetDoctor(audit.DoctorId);
            m_DoctorManagerService.OnUpdatingHandler = (expDoctor, a) =>
            {
                ((Person)expDoctor).Doctor.AuditStatus = audit.IsPass ? DoctorAuditStatus.Pass : DoctorAuditStatus.Fail;
                ((Person)expDoctor).Doctor.AuditDate = DateTime.Now;
            };
            var updatedStatus = await m_DoctorManagerService.UpdateObject<Person>(isAsync: true);
            if (updatedStatus == AppServiceExecuteStatus.Success)
            {
                jsonResult.Result = true;
                return Json(jsonResult);
            }

            jsonResult.Err = string.Format(updatedStatus.ToDescription(), "审核失败");
            return Json(jsonResult);
        }

        [HttpPost]
        public async Task<JsonResult> BindHospital(VmDoctorBind preBind)
        {
            var jsonResult = new VM_JsonOnlyResult();
            var modelState = (new VmDoctorBindValidator()).Validate(preBind);
            if (!modelState.IsValid)
            {
                jsonResult.Value = -1001;
                jsonResult.Err = string.Join(",", modelState.Errors.Select(m => m.ErrorMessage));
                return await Task.FromResult(Json(jsonResult));
            }

            var toBindHospital = m_HospitalManagerService.GetHospital(preBind.HospitalId);
            if (toBindHospital == null)
            {
                jsonResult.Value = -1002;
                jsonResult.Err = "绑定医院不存在";
                return await Task.FromResult(Json(jsonResult));
            }
            m_DoctorManagerService.PreOnUpdateHandler = () => m_DoctorManagerService.GetDoctor(preBind.DoctorId);
            m_DoctorManagerService.OnUpdatingHandler = (doctor, newDoctor) =>
            {
                var expDoctor = (Person) doctor;
                var preBindInfo = expDoctor.Doctor.InHospital?.FirstOrDefault(h =>
                    h.ID == preBind.PreBindId && h.PersonDoctorId == preBind.DoctorId);
                if (preBindInfo != null)
                {
                    preBindInfo.BindedHospitalId = toBindHospital.ID;
                    preBindInfo.BindedHospitalName = toBindHospital.Name;
                    preBindInfo.IsBinded = true;
                }
            };
            var updatedStatus = await m_DoctorManagerService.UpdateObject<Person>(isAsync: true);
            if (updatedStatus == AppServiceExecuteStatus.Success)
            {
                jsonResult.Result = true;
                return Json(jsonResult);
            }
            jsonResult.Err = string.Format(updatedStatus.ToDescription(), "医院绑定失败");
            return Json(jsonResult);
        }

        #endregion

        #region 员工管理

        [HttpGet]
        public async Task<ActionResult> UserList(string filter, int? p)
        {
            var staffIndex = new VmUserStaffIndex() {Filter = filter?.Trim()};
            var pageIndex = p.HasValue ? p.Value : 1;
            var searchExp = staffIndex.CombineExpression();
            var entList =
                await m_UserManagerService.GetStaffListAsync(
                    searchExp,
                    UserSortBy<Person, JsonSortValue>(ConfigHelper.KeyModuleStaffSort),
                    pageIndex);
            staffIndex.TotalRecordCount = await m_UserManagerService.GetStaffCount(searchExp);
            staffIndex.ViewObjects = entList.Select(item => new VmStaff()
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
            staffIndex.CurrentIndex = pageIndex;
            staffIndex.DepartmentNameList = (await m_DepartmentManagerService.GetDepartments()).Select(d => d.Name);
            staffIndex.RoleNameList = (await m_RoleManagerService.GetRoles()).Select(r => r.Name);
            staffIndex.DepartmentNameListJsonString =
                JsonConvert.SerializeObject(staffIndex.DepartmentNameList.Select(n => new {id = n, name = n}).ToList());
            staffIndex.RoleNameListJsonString =
                JsonConvert.SerializeObject(staffIndex.RoleNameList.Select(n => new {id = n, name = n}).ToList());
            return View(staffIndex);
        }

        [HttpPost]
        public async Task<JsonResult> AddStaff(VmStaff staff)
        {
            var ret = new VM_JsonOnlyResult();
            var modelState = (new VmStaffValidator()).Validate(staff);
            if (!modelState.IsValid)
            {
                ret.Value = -1001;
                ret.Err = string.Join(",", modelState.Errors.Select(m => m.ErrorMessage));
                return await Task.FromResult(Json(ret));
            }

            m_UserManagerService.PreOnAddHandler =
                () => !m_UserManagerService.UserIsExists(staff.NagigatedDomainObject.Name);

            var dep = m_DepartmentManagerService.GetDepartment(staff.NagigatedDomainObject.DepartmentName);
            staff.NagigatedDomainObject.DepartmentId = dep?.ID;

            var role = m_UserManagerService.GetRole(staff.NagigatedDomainObject.Role.Name);
            staff.NagigatedDomainObject.Role = role;
            staff.NagigatedDomainObject.RoleId = role?.ID;

            var added = await m_UserManagerService.AddObject(staff.NagigatedDomainObject, true);
            if (added == AppServiceExecuteStatus.Success)
            {
                ret.Value = staff.NagigatedDomainObject.ID;
                ret.Result = true;
                return Json(ret);
            }

            ret.Err = string.Format(added.ToDescription(), "员工信息已存在");
            return Json(ret);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteStaff(long staffId)
        {
            var ret = new VM_JsonOnlyResult();
            var deleted = await m_UserManagerService.LogicObjectDelete<Person, long>(staffId, true);
            if (deleted == AppServiceExecuteStatus.Success)
            {
                ret.Value = staffId;
                ret.Result = true;
                return Json(ret);
            }

            ret.Err = string.Format(deleted.ToDescription(), "员工信息不存在");
            return Json(ret);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStaff(VmStaff staff)
        {
            var ret = new VM_JsonOnlyResult();
            var modelState = (new VmStaffValidator()).Validate(staff);
            if (!modelState.IsValid)
            {
                ret.Value = -1001;
                ret.Err = string.Join(",", modelState.Errors.Select(m => m.ErrorMessage));
                return await Task.FromResult(Json(ret));
            }

            m_UserManagerService.PreOnUpdateHandler =
                () =>
                {
                    var selfUser = m_UserManagerService.GetUser(staff.NagigatedDomainObject.ID);
                    if (selfUser == null)
                        return null;
                    var otherUser = m_UserManagerService.GetUser(staff.NagigatedDomainObject.Name);
                    if (otherUser == null || otherUser.ID == selfUser.ID)
                        return selfUser;
                    return null;
                };
            m_UserManagerService.OnUpdatingHandler = (existOject, newObject) =>
            {
                var n = VmPersonMapper.MapTo(((Person) newObject), (Person) existOject);
                var dep = m_DepartmentManagerService.GetDepartment(staff.NagigatedDomainObject.DepartmentName);
                n.DepartmentId = dep?.ID;

                var role = m_UserManagerService.GetRole(staff.NagigatedDomainObject.Role.Name);
                n.Role = role;
                n.RoleId = role?.ID;
            };
            var updatedRet = await m_UserManagerService.UpdateObject(staff.NagigatedDomainObject, true);
            if (updatedRet == AppServiceExecuteStatus.Success)
            {
                ret.Value = staff.NagigatedDomainObject.ID;
                ret.Result = true;
                return Json(ret);
            }

            ret.Err = string.Format(updatedRet.ToDescription(), "更新员工信息失败");
            return Json(ret);
        }

        #endregion
    }
}