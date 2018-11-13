using System.Web;
using System.Web.Mvc;
using HSMY.Web.General.MVC;
using HSMY.Web.General.WebAPI;

namespace HSMY_WxWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new GlobalDisposeFilterAttribute());
        }
    }
}
