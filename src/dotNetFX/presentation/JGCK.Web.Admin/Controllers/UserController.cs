using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JGCK.Modules.Membership;
using JGCK.Web.Admin.Models;
using JGCK.Web.General;

namespace JGCK.Web.Admin.Controllers
{
    public class UserController : JGCK_MvcController
    {
        private UserManager m_UserManagerService { get; set; }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(VmUserLogin userLogin)
        {
            var ret = await m_UserManagerService.CheckAsync(userLogin.UserName, userLogin.Pwd);
            if (ret == CheckUserPwdResult.Success)
            {
                return RedirectToAction("", "");
            }

            return View(userLogin);
        }
    }
}