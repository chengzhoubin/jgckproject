using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Web.General.VO;

namespace JGCK.Web.General.MVC
{
    public abstract class AbstractVoWithFilter<TFilter, TVo> : AbstractPageVO
        where TVo : class
    {
        public TFilter Filter { get; set; }

        public IList<TVo> ViewObjects { get; set; }

        public TVo AddOrUpdateViewObject { get; set; }
    }
}
