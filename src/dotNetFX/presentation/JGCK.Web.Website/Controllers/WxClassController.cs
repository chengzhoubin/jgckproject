using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using HSMY.BizLogic.Org;
using HSMY.BizLogic.Org.Enums;
using HSMY.Data.MicroClass;
using HSMY.Util;
using HSMY.Web.General.MVC;
using HSMY.Web.General.VO;
using HSMY.Web.General.WebAPI;
using HSMY_WxWeb.Models;

namespace HSMY_WxWeb
{
    public class WxClassController : HSMY_ApiController
    {
        private IClassService MClassService { get; set; }

        private DoctorService MDoctorService { get; set; }

        private MembershipService MUserService { get; set; }

        private HospitalService MHospitalService { get; set; }

        /// <summary>
        /// 根据条件获取课程
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetWxClasses")]
        public List<VM_Wx_ClassInfo> GetWxClasses(VM_Wx_ClassFilter f)
        {
            if (f.CourseId > 0 && f.IsAddViewCount)
            {
                var addViewCountClass = MClassService.GetMicroclass(f.CourseId);
                addViewCountClass.view_count++;
                MClassService.UpdateMicroclass(addViewCountClass, false);
            }

            var queryClasses = (IQueryable<microclass>) MClassService.GetMicroclasses(m => !m.is_deleted);
            queryClasses = queryClasses.Where(f.CombineExpression());
            return f.GetPageAndSortItiteral(queryClasses).Select(m =>
            {
                return new VM_Wx_ClassInfo(m,
                    id => MDoctorService.GetDoctor(id),
                    hid => MHospitalService.GetHospitalProfileByID(hid),
                    cid => MClassService.GetOnlineUserCountByType(cid, 0));
            }).ToList();
        }

        /// <summary>
        /// 根据条件获取记录总数
        /// 为终端分页
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetWxClassesQuantity")]
        public JsonResultGenerics<int> GetWxClassesQuantity(VM_Wx_ClassFilter f)
        {
            var queryClasses = (IQueryable<microclass>) MClassService.GetMicroclasses(m => !m.is_deleted);
            queryClasses = queryClasses.Where(f.CombineExpression());
            return new JsonResultGenerics<int>
            {
                Result = true,
                Value = queryClasses.Count()
            };
        }

        /// <summary>
        /// 用户收藏课程
        /// </summary>
        /// <param name="userClass"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ToCollectClass")]
        public async Task<JsonResultGenerics<bool>> ToCollectClass(VM_Wx_User_Class userClass)
        {
            userClass.ChangeUnionId2UserId(strUnionId => MUserService.GetMember(unionId: strUnionId));
            var collectResult = await MUserService.MemberFavoriteMicrocalss(userClass.UserId, userClass.ClassId);
            var ret = new JsonResultGenerics<bool>();
            if (collectResult == MemberFavoriteClassResult.Success)
                ret.Result = true;
            else
                ret.Error = collectResult.ToDescription();
            return ret;
        }

        /// <summary>
        /// 获取用户收藏的课程
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserFavoriteClasses")]
        public async Task<List<VM_Wx_ClassInfo>> GetUserFavoriteClasses(long userId)
        {
            var userClassIds = await MUserService.GetMemberFavoriteMicrocalssIds(userId);
            if (userClassIds == null || userClassIds.Count == 0)
                return new List<VM_Wx_ClassInfo>();
            return MClassService.GetMicroclasses(m => userClassIds.Contains(m.id) && !m.is_deleted).Select(m =>
              {
                  var vo = new VM_Wx_ClassInfo(m,
                      id => MDoctorService.GetDoctor(id),
                      hid => MHospitalService.GetHospitalProfileByID(hid),
                      cid => MClassService.GetOnlineUserCountByType(cid, 0));
                  return vo;
              }).ToList();
        }

        /// <summary>
        /// 取消用户收藏的课程
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CancelUserFavoriteClass")]
        public async Task<JsonResultGenerics<bool>> CancelUserFavoriteClass(VM_Wx_User_Class userClass)
        {
            userClass.ChangeUnionId2UserId(strUnionId => MUserService.GetMember(unionId: strUnionId));
            await MUserService.CancelFavoriteMicrocalss(userClass.UserId, userClass.ClassId);
            return new JsonResultGenerics<bool>
            {
                Value = true,
                Result = true
            };
        }
    }
}