using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JGCK.Web.General.MVC
{
    /// <summary>
    /// 全局用户授权
    /// </summary>
    public class GlobalUserAuthorizationFilterAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var curExecutingControllerType = filterContext.Controller.GetType();
            var actionMethod = curExecutingControllerType.GetMethod(filterContext.ActionDescriptor.ActionName);
            //TODO:
            
        }
    }
}
