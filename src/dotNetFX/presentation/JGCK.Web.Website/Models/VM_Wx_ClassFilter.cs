using HSMY.Data.MicroClass;
using HSMY.Util;
using HSMY.Util.Enums;
using HSMY.Web.General.MVC;
using HSMY.Web.General.VO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using HSMY_WxWeb.Enums;

namespace HSMY_WxWeb.Models
{
    /// <summary>
    /// 课程搜索条件
    /// </summary>
    public class VM_Wx_ClassFilter : AbstractPageVO, ICustomFilter<microclass>
    {
        /// <summary>
        /// 课程ID
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public Operation Op { get; set; }

        ///// <summary>
        ///// 当前时间节点
        ///// </summary>
        public string CurrentBaseTimeString { get; set; }

        ///// <summary>
        ///// 是否置顶
        ///// </summary>
        public bool IsTop { get; set; } = false;

        ///// <summary>
        ///// 是否精选
        ///// </summary>
        public bool IsFeatured { get; set; } = false;

        ///// <summary>
        ///// 排序
        ///// </summary>
        public AscOrDesc Sort { get; set; }

        /// <summary>
        /// 是否添加ViewCount
        /// </summary>
        public bool IsAddViewCount { get; set; } = true;

        public StartTimeOrEndTimeCorn PreviousCurrentCorn { get; set; }

        public Expression<Func<microclass, bool>> CombineExpression()
        {
            var ret = PredicateBuilder.True<microclass>();
            ret = ret.And(m => m.is_hot == IsFeatured);
            ret = ret.And(m => m.is_top == IsTop);
            if (CourseId > 0)
            {
                ret = ret.And(m => m.id == CourseId);
            }

            if (!string.IsNullOrEmpty(CurrentBaseTimeString))
            {
                var splitTime = Convert.ToDateTime(CurrentBaseTimeString);
                if (Op == Operation.EqualTo)
                    ret = ret.And(m => m.start_time == splitTime);
                else if (Op == Operation.LessAndEqualTo)
                    ret = ret.And(m => m.start_time <= splitTime);
                else if (Op == Operation.LessThan)
                    ret = ret.And(m => m.start_time < splitTime);
                else if (Op == Operation.MoreAndEqualTo)
                    ret = ret.And(m => m.start_time >= splitTime);
                else if (Op == Operation.Morethan)
                    ret = ret.And(m => m.start_time > splitTime);
            }

            switch (PreviousCurrentCorn)
            {
                case StartTimeOrEndTimeCorn.IsFine:
                    ret = ret.And(m => !m.end_time.HasValue);
                    break;
                case StartTimeOrEndTimeCorn.EndTimeIsNotNull:
                    ret = ret.And(m => m.end_time.HasValue);
                    break;
            }
            return ret;
        }

        public IEnumerable<microclass> GetPageAndSortItiteral(IQueryable<microclass> source)
        {
            if (Sort == AscOrDesc.Asc)
                source = source.OrderBy(m => m.start_time);
            else if (Sort == AscOrDesc.Desc)
                source = source.OrderByDescending(m => m.start_time);

            var skip = (CurrentIndex - 1) * PageSize;
            source = source.Skip(skip).Take(PageSize).AsNoTracking();
            return source;
        }
    }
}