using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using HSMY.Web.General.WebAPI;

namespace HSMY_WxWeb
{
    public class WebApiFilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(new ApiGlobalDisposeFilterAttribute());
        }
    }
}