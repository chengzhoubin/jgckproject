using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HSMY_AdminWeb.Models;
using JGCK.Framework.EF;
using JGCK.Modules.Membership;
using JGCK.Respority.UserWork;
using JGCK.Util.Enums;
using JGCK.Web.Admin.Models;
using JGCK.Web.General;
using JGCK.Web.General.MVC;
using Newtonsoft.Json;

namespace JGCK.Web.Admin.Controllers
{
    public class UserController : JGCK_MvcController
    {
        protected override string m_ModuleName => "sf_doctorData";

        private AbstractUnitOfWork.OrderByExpression<Person>[] UserSortBy
        {
            get
            {
                var keyOfSort = $"{m_ModuleName}_sort_keys";
                var jsonSortValue = CookieHelper.GetValue<List<JsonSortValue>>(keyOfSort, false);
                var orderByExps = new List<AbstractUnitOfWork.OrderByExpression<Person>>();
                jsonSortValue?.ForEach(v =>
                {
                    var item = new AbstractUnitOfWork.OrderByExpression<Person>();
                    item.OrderByExpressionMember = v.SortProperty;
                    item.SortBy = v.SortDirect;
                    orderByExps.Add(item);
                });

                if (jsonSortValue == null || orderByExps.Count == 0)
                {
                    orderByExps.Add(new AbstractUnitOfWork.OrderByExpression<Person>
                    {
                        OrderByExpressionMember = "ID",
                        SortBy = AscOrDesc.Desc
                    });
                    orderByExps.Add(new AbstractUnitOfWork.OrderByExpression<Person>
                    {
                        OrderByExpressionMember = "Doctor.AuditStatus",
                        SortBy = AscOrDesc.Desc
                    });
                }

                return orderByExps.ToArray();
            }
        }

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
            var doctorIndex = new VmUserDoctorIndex() {Filter = filter?.Trim()};
            var pageIndex = p.HasValue ? p.Value : 1;
            var searchExp = doctorIndex.CombineExpression();
            var entList = await m_DoctorManagerService.GetDoctorListAsync(searchExp, UserSortBy, pageIndex);
            doctorIndex.TotalRecordCount = await m_DoctorManagerService.GetDoctorCount(searchExp);
            doctorIndex.ViewObjects = entList.Select(item => new VmUserDoctor
            {
                NagigatedDoctor = item,
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
            var vm = new VM_JsonOnlyResult();
            var ret = await m_DoctorManagerService.UpdateDoctorInfo(doctor.NagigatedDoctor);
            vm.Result = ret > 0;
            return Json(vm);
        }

        #endregion

        [HttpGet]
        public ActionResult UserList()
        {
            return View();
        }
    }
}