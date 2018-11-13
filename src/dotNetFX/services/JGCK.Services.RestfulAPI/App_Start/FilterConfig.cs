using System.Web;
using System.Web.Mvc;

namespace JGCK.Services.RestfulAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
