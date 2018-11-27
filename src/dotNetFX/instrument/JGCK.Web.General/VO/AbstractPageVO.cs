using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using JGCK.Web.General.MVC;

namespace JGCK.Web.General.VO
{
    public abstract class AbstractPageVO : AbstractVO
    {
        public int CurrentIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalRecordCount { get; set; } = 0;

        public int TotalPages
        {
            get { return Convert.ToInt32(Math.Ceiling(TotalRecordCount / Convert.ToDecimal(PageSize))); }
        }
    }
}
