using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using HSMY.BizLogic.Org;
using HSMY.BizLogic.Org.Enums;
using HSMY.Data.Membership.Models;
using HSMY.Util;
using HSMY.Util.Helper;
using HSMY.Util.MessagePusher.SMS;
using HSMY.Web.General.VO;
using HSMY.Web.General.WebAPI;
using HSMY_WxWeb.Models;

namespace HSMY_WxWeb.Controllers
{
    public class WxUserController : HSMY_ApiController
    {
        private MembershipService MUserService { get; set; }

        [HttpPost]
        public JsonResultGenerics<Guid> CreateImageCode()
        {
            var userToken = new member_token {type = 1};
            return new JsonResultGenerics<Guid>
            {
                Result = true,
                Value = MUserService.CreateToken(userToken,CodeType.NumeralUppercaseLetters).tokenid
            };
        }

        [HttpPost]
        public JsonResultGenerics<Guid> CreateSmsCode(VM_Wx_SmsCode smsCode)
        {
            if (MUserService.GetMember(phone: smsCode.Phone) != null)
            {
                return new JsonResultGenerics<Guid>()
                {
                    Result = false,
                    Error = "输入的手机号已经存在",
                    Value = Guid.Empty
                };
            }

            var userToken = new member_token
            {
                type = 2,
                fromsource = smsCode.Phone,
                fromsourcetype = 1
            };
            userToken = MUserService.CreateToken(userToken, length: 6);
            //TODO:调用短信接口
            Hashtable sendParam = new Hashtable
            {
                { "vCode", userToken.tokendata }
            };
            AlidySmsSender.toSend(smsCode.Phone, sendParam);
            //---------------
            return new JsonResultGenerics<Guid>
            {
                Result = true,
                Value = userToken.tokenid
            };
        }

        /// <summary>
        /// 用户是否已经绑定手机号
        /// </summary>
        /// <param name="toSend"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResultGenerics<bool> HasBindedPhone(VM_Wx_SmsCode toSend)
        {
            var ret = new JsonResultGenerics<bool>();
            var wxUser = MUserService.GetMember(id: toSend.UserID, unionId: toSend.UnionID);
            if (wxUser == null)
            {
                ret.Error = "用户不存在";
                return ret;
            }
            ret.Result = true;
            ret.Value = !string.IsNullOrEmpty(wxUser?.phone);
            return ret;
        }

        [HttpPost]
        public JsonResultGenerics<bool> Validate(VM_Wx_Token_Validate val)
        {
            var realUserToken = MUserService.GetToken(val.TokenId);
            var retValidate = MUserService.IsValidateToken(realUserToken, val.UserToken,
                (updatedUserToken) =>
                {
                    if (!val.UserId.HasValue)
                        return;
                    var anUser = MUserService.GetMember(id: val.UserId.Value);
                    if (anUser == null)
                        return;
                    anUser.modify_date = DateTime.Now;
                    anUser.phone = realUserToken.fromsource;
                    MUserService.AddOrUpdateUserInfo(anUser);
                });
            return new JsonResultGenerics<bool>
            {
                Result = true,
                Value = retValidate == MemberValidateCodeResult.Pass,
                Error = retValidate.ToDescription()
            };
        }
    }
}