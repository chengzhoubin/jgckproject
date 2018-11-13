using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HSMY.BizLogic.Org;
using HSMY.Util.Helper;
using HSMY.Web.General;

namespace HSMY_WxWeb.Controllers
{
    [RoutePrefix("mvc/ajax")]
    public class MvcAjaxController : HSMY_MvcController
    {
        private MembershipService MUserService { get; set; }

        [HttpGet]
        [Route("imagecode")]
        public void ImageCode(string token)
        {
            var userToken = MUserService.GetToken(Guid.Parse(token));
            byte[] Byt_Img = CodeHelper.CreateCodeImage(pStr_Type: CodeType.Numeral, checkCode: userToken.tokendata);
            Response.ClearContent();
            Response.ContentType = "image/Gif";
            Response.BinaryWrite(Byt_Img);
        }
    }
}