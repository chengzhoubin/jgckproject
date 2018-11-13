using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Framework
{
    /// <summary>
    /// 运行策略
    /// </summary>
    public interface IToRunPolicy<TPolicyObject>
    {
        /// <summary>
        /// 序列化存储对象
        /// </summary>
        object SerializePolicyObject { get; set; }

        /// <summary>
        /// 获取策略之前执行
        /// </summary>
        Action OnPreReserveHandle { get; set; }

        /// <summary>
        /// 获取策略之后执行
        /// </summary>
        Action<TPolicyObject> OnReservedHandle { get; set; }

        /// <summary>
        /// 获取策略
        /// </summary>
        /// <param name="getPolicyHandleFunc"></param>
        /// <returns></returns>
        TPolicyObject Reserve(Func<TPolicyObject, object> getPolicyHandleFunc);

        /// <summary>
        /// 异步获取策略
        /// </summary>
        /// <param name="getPolicyHandleFunc"></param>
        /// <returns></returns>
        Task<TPolicyObject> ReserveAsync(Func<TPolicyObject, object> getPolicyHandleFunc);
    }
}
