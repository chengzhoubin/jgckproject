using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HSMY.Data.Membership;

namespace HSMY_WxWeb.Models
{
    /// <summary>
    /// 用户收藏课程
    /// </summary>
    public class VM_Wx_User_Class
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string UnionID { get; set; }

        public void ChangeUnionId2UserId(Func<string, member> convertHandle)
        {
            if (string.IsNullOrEmpty(this.UnionID))
                return;
            var oneMember = convertHandle(UnionID);
            if (oneMember == null)
            {
                throw new NullReferenceException("用户标识无法找到用户信息");
            }
            if (this.UserId != oneMember.id)
            {
                this.UserId = oneMember.id;
            }
        }
    }
}