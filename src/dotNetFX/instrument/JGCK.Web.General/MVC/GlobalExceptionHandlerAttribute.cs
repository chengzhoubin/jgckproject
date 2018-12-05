using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JGCK.Web.General.MVC
{
    public class GlobalExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var exp = filterContext.Exception;
            ProcessInnerException(exp);
            if (filterContext.ExceptionHandled)
                return;

            var httpExp = new HttpException(null, exp);
            filterContext.HttpContext.Response.StatusCode = httpExp.GetHttpCode();
            filterContext.Result = new RedirectResult("~/shared/error?code=" + httpExp.GetHttpCode());
            filterContext.ExceptionHandled = true;
        }

        private Exception ProcessInnerException(Exception exp)
        {
            if (exp == null)
                return null;
            LogHelper.LogError(exp.ToString());
            if (exp.InnerException == null)
                return null;
            return ProcessInnerException(exp.InnerException);
        }
    }
}
