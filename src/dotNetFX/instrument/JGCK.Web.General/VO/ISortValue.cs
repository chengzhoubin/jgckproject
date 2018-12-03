using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGCK.Util.Enums;

namespace JGCK.Web.General.VO
{
    public interface ISortValue
    {
        string SortProperty { get; set; }

        AscOrDesc SortDirect { get; set; }
    }
}
