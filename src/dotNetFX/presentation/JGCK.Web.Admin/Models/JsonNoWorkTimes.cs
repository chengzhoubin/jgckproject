using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JGCK.Web.Admin.Models
{
    /// <summary>
    /// 自定义休息时间
    /// </summary>
    public class JsonNoWorkTimes
    {
        public int Year { get; set; }

        public JsonNoWorkDaysInMonth[] RestMonths { get; set; }
    }

    public class JsonNoWorkDaysInMonth
    {
        /// <summary>
        /// 休息月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 当月几号休息
        /// </summary>
        public int[] Days { get; set; }
    }
}