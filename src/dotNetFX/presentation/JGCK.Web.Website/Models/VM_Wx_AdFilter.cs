using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using HSMY.Data.Advertisement;
using HSMY.Util;
using HSMY.Web.General.MVC;
using HSMY.Web.General.VO;

namespace HSMY_WxWeb.Models
{
    public class VM_Wx_AdFilter : AbstractPageVO, ICustomFilter<ad>
    {
        /// <summary>
        /// AdId
        /// </summary>
        public long? AdId { get; set; }

        /// <summary>
        /// 0表示 全部
        /// 1表示 首页大眼睛
        /// 2表示 首页专题
        /// </summary>
        public int AdType { get; set; }

        public Expression<Func<ad, bool>> CombineExpression()
        {
            var exp = PredicateBuilder.Create<ad>(m => !m.is_deleted);
            if (AdType > 0)
            {
                exp = exp.And(m => m.ad_position_id == AdType);
            }
            if (AdId.HasValue && AdId > 0)
            {
                exp = exp.And(m => m.id == AdId);
            }
            return exp;
        }
    }
}