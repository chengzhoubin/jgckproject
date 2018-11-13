using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSMY_WxWeb.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum StartTimeOrEndTimeCorn
    {
        /// <summary>
        /// 
        /// </summary>
        Null = 0,

        /// <summary>
        /// 课程已经开始
        /// </summary>
        IsFine = 1,

        /// <summary>
        /// 结束时间不为空
        /// </summary>
        EndTimeIsNotNull
    }
}