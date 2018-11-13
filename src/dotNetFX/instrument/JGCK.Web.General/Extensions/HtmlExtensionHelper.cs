using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JGCK.Web.General.Extensions
{
    public static class HtmlExtensionHelper
    {
        public static IList<SelectListItem> AddEmptyOption(this IEnumerable<SelectListItem> source, string text = "请选择",
            string value = "0")
        {
            var ret = source.ToList();
            ret.Insert(0, new SelectListItem {Text = text, Value = value});
            return ret;
        }
    }
}
