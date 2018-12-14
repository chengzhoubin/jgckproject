using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Framework
{
    public interface IAppService : IDisposable
    {
        /// <summary>
        /// 是否满足逻辑删除条件
        /// </summary>
        Func<bool> PreLogicDeleteHandler { get; set; }

        Func<bool> PreOnAddHandler { get; set; }
        Func<object> PreOnUpdateHandler { get; set; }
        Action<object, object> OnUpdatingHandler { get; set; }
    }

    /// <summary>
    /// 应用层执行结果
    /// </summary>
    public enum AppServiceExecuteStatus
    {
        /// <summary>
        /// 准备执行
        /// </summary>
        [Description("准备执行")]
        Pending,

        /// <summary>
        /// 执行中
        /// </summary>
        [Description("执行中")]
        Executing,

        /// <summary>
        /// 执行成功
        /// </summary>
        [Description("执行成功")]
        Success,

        /// <summary>
        /// 执行失败
        /// </summary>
        [Description("{0}，请重新输入！")]
        Fail = -1001,

        /// <summary>
        /// 在执行过程中，条件不符合无法执行
        /// </summary>
        [Description("{0}，请重新输入！")]
        DoNotContinue = -1002
    }
}
