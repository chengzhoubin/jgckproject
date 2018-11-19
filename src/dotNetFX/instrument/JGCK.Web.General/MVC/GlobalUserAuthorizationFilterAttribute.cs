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
            if (SkipControllerAction(filterContext))
                return;

            var token = JGCKUserToken.ResolveNewToken();
            if (token == null)
            {
                filterContext.Result = new RedirectResult("~/User/Login");
                return;
            }

            var curExecutingControllerType = filterContext.Controller.GetType();
            var actionMethod = curExecutingControllerType.GetMethod(filterContext.ActionDescriptor.ActionName);
            //TODO:
            
        }

        private bool SkipControllerAction(ActionExecutingContext filterContext)
        {
            var ctrlName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = filterContext.ActionDescriptor.ActionName;
            return ctrlName == "User" && actionName == "Login";
        }
    }
}
