using System.Web;
using System.Web.Mvc;
using JGCK.Web.General.MVC;

namespace JGCK.Web.Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalUserAuthorizationFilterAttribute());
            //filters.Add(new GlobalDisposeFilterAttribute());
        }
    }
}
