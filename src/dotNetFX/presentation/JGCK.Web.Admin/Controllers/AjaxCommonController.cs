using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HSMY_AdminWeb.Models;
using JGCK.Web.Admin.Models;
using JGCK.Web.General;
using JGCK.Web.General.VO;

namespace JGCK.Web.Admin.Controllers
{
    public class AjaxCommonController : JGCK_MvcController
    {
        public JsonResult SetSort(JsonSortRequest sort)
        {
            var sortKey = $"{sort.ModuleName}_sort_keys";
            var retSort = CookieHelper.GetValue<List<JsonSortValue>>(sortKey, false) ?? new List<JsonSortValue>();
            retSort.Add(new JsonSortValue
            {
                SortProperty = sort.SortProperty,
                SortDirect = sort.SortDirect
            });
            CookieHelper.CreateCookieJsonValue(retSort, sortKey);
            return Json(new VM_JsonOnlyResult
            {
                Result = true,
                Value = retSort
            });
        }

        public JsonResult RemoveSort(string moduleKey)
        {
            CookieHelper.RemoveIt($"{moduleKey}_sort_keys");
            return Json(new VM_JsonOnlyResult {Result = true});
        }
    }
}