using HSMY_AdminWeb.Models;
using JGCK.Framework;
using JGCK.Modules.Membership;
using JGCK.Respority.UserWork;
using JGCK.Util;
using JGCK.Web.Admin.Models;
using JGCK.Web.Admin.Models.Mapper;
using JGCK.Web.General;
using JGCK.Web.General.Helper;
using JGCK.Web.General.MVC;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JGCK.Web.Admin.Controllers
{
    public class UserController : JGCK_MvcController
    {
        private UserManager m_UserManagerService { get; set; }
        private DoctorManager m_DoctorManagerService { get; set; }

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
            var doctorIndex = new VmUserDoctorIndex() { Filter = filter?.Trim() };
            var pageIndex = p.HasValue ? p.Value : 1;
            var searchExp = doctorIndex.CombineExpression();
            var entList =
                await m_DoctorManagerService.GetDoctorListAsync(searchExp, UserSortBy<Person, JsonSortValue>(ConfigHelper.KeyModuleDoctorSort),
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

            jsonResult.Err = updateStatus.ToDescription();
            return Json(jsonResult);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteDoctor(long doctorId)
        {
            var jsonResult = new VM_JsonOnlyResult();
            m_DoctorManagerService.PreLogicDeleteHandler = () => m_DoctorManagerService.GetDoctor(doctorId) != null;
            var deleteStatus = await m_DoctorManagerService.LogicObjectDelete<Person, long>(doctorId);
            if (deleteStatus == AppServiceExecuteStatus.Success)
            {
                jsonResult.Result = true;
                return Json(jsonResult);
            }

            jsonResult.Err = deleteStatus.ToDescription();
            return Json(jsonResult);
        }

        #endregion

        [HttpGet]
        public async Task<ActionResult> UserList(string filter, int? p)
        {
            var staffIndex = new VmUserStaffIndex() { Filter = filter?.Trim() };
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
            var added = await m_UserManagerService.AddObject(staff.NagigatedDomainObject, true);
            if (added == AppServiceExecuteStatus.Success)
            {
                ret.Value = staff.NagigatedDomainObject.ID;
                ret.Result = true;
                return Json(ret);
            }

            ret.Err = added.ToDescription();
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

            ret.Err = deleted.ToDescription();
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
                () => m_UserManagerService.GetUser(staff.NagigatedDomainObject.ID);
            m_UserManagerService.OnUpdatingHandler = (existOject, newObject) =>
                {
                    VmPersonMapper.MapTo(((Person) newObject), (Person) existOject);
                };
            var updatedRet = await m_UserManagerService.UpdateObject(staff.NagigatedDomainObject, true);
            if (updatedRet == AppServiceExecuteStatus.Success)
            {
                ret.Value = staff.NagigatedDomainObject.ID;
                ret.Result = true;
                return Json(ret);
            }

            ret.Err = updatedRet.ToDescription();
            return Json(ret);
        }
    }
}