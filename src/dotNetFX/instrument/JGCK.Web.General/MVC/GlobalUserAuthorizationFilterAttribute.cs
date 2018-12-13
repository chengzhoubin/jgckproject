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

            var ctx = filterContext.HttpContext;
            var token = JGCKUserToken.ResolveNewToken();
            if (token == null)
            {
                var redirectURL = "~/User/Login";
                if (!string.IsNullOrEmpty(ctx.Request.RawUrl) && ctx.Request.RawUrl != "/")
                {
                    redirectURL += "?reloadurl=" + ctx.Server.UrlEncode(ctx.Request.RawUrl);
                }
                filterContext.Result = new RedirectResult(redirectURL);
                return;
            }

            var curExecutingControllerType = filterContext.Controller.GetType();
            var actionMethod = curExecutingControllerType.GetMethods()
                .Where(mi => mi.Name == filterContext.ActionDescriptor.ActionName);
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
