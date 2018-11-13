using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using HSMY.BizLogic.Org;
using HSMY.Util.Helper;
using HSMY.Web.General;
using HSMY.Util.WeiXin;
using HSMY.Util.WenXin;
using HSMY.Web.General.HttpModules;
using HSMY.Web.General.VO;
using HSMY.Web.Security.Token;
using Newtonsoft.Json;

namespace HSMY_WxWeb.Controllers
{
    public class HomeController : HSMY_MvcController
    {
        private static readonly Regex PathQueryClassDetail =
            new Regex(@"^\/course\/(\d+)(\/)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private string ClientRoute => Request["cp"];

        private IClassService MClassService { get; set; }

        //private MembershipService MUserService { get; set; }
        public ActionResult Index()
        {
            ViewBag.sitetype = "wx";
            if (!WxHelper.IsMicroMessagerBrowser)
            {
                if (!string.IsNullOrEmpty(Request["path"]))
                {
                    return Redirect(ModuleConfiguration.ConfigSection.JumpBaseUrlHost + "/#" + Request["path"]);
                }
                ViewBag.sitetype = "m";
                return View(GetAnnoyUserToken());
            }

            if (string.IsNullOrEmpty(ClientRoute))
            {
                goto getTokenRegion;
            }

            if (PathQueryClassDetail.IsMatch(ClientRoute))
            {
                var strClassId = ClientRoute.ToLower().Replace("/course/", "").Trim('/');
                long iClassId = 0;
                long.TryParse(strClassId, out iClassId);
                if (iClassId == 0)
                {
                    goto getTokenRegion;
                }

                string strJumpUrl = "";
                var shareClass = MClassService.GetMicroclass(iClassId);
                if (shareClass != null && !shareClass.is_deleted &&
                    (shareClass.is_fine || shareClass.end_time.HasValue))
                {
                    strJumpUrl = ModuleConfiguration.ConfigSection.JumpBaseUrlHost + "/#/live/" + strClassId;
                }
                else
                {
                    strJumpUrl = ModuleConfiguration.ConfigSection.JumpBaseUrlHost + "/#" + ClientRoute;
                }
                return Redirect(strJumpUrl);
            }
            else
            {
                return Redirect(ModuleConfiguration.ConfigSection.JumpBaseUrlHost + "/#" + ClientRoute);
            }

            getTokenRegion:
            WxUserToken userToken = WxUserToken.GetToken();
            return View(userToken);
        }
        
        private WxUserToken GetAnnoyUserToken()
        {
            return new WxUserToken()
            {
                unionid = "7827035bc5a243a8b82ab0a00341d030",
                userId = 2,
                nickname = "匿名用户",
                headimgurl = "http://resource.bestdoctor1.com/resources_4441dbc6-6b37-439d-a412-b0d8c75e7ec7.png"
            };
        }
    }
}
