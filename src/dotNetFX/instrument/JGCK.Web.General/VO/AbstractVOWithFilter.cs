using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGCK.Web.General.MVC
{
    public abstract class AbstractVOWithFilter<TFilter, TVO>
        where TVO : class
    {
        public TFilter Filter { get; set; }

        public IList<TVO> ViewObjects { get; set; }

        public TVO AddOrUpdateViewObject { get; set; }
    }
}
