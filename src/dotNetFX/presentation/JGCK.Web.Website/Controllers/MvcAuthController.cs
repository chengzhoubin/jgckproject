using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HSMY.BizLogic.Org;
using HSMY.Util.Helper;
using HSMY.Web.General;
using HSMY.Util.WeiXin;
using HSMY.Web.General.VO;
using Newtonsoft.Json;

namespace HSMY_WxWeb.Controllers
{
    [RoutePrefix("mvc/auth")]
    public class MvcAuthController : HSMY_MvcController
    {
        //private MembershipService MUserService { get; set; }
        [Route("noauthen")]
        public ActionResult ErrorNoAuthen(string error)
        {
            return Content(error);
        }

        [Route("Success")]
        public ActionResult SuccessAuthen()
        {
            return Content("Success");
        }

        [Route("gettoken")]
        public JsonResult GetUserToken()
        {
            var token = WxUserToken.GetToken();
            var ret = new JsonResultGenerics<string>();
            if (token != null)
            {
                ret.Value = JsonConvert.SerializeObject(token);
                ret.Result = true;
            }
            else
            {
                ret.Error = "UserToken已过期";
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}